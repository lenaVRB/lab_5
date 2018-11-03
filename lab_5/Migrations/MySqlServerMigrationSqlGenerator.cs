using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace lab_5.Migrations
{
	class MySqlServerMigrationSqlGenerator : SqlServerMigrationsSqlGenerator
	{
		public MySqlServerMigrationSqlGenerator(
		MigrationsSqlGeneratorDependencies dependencies,
		IMigrationsAnnotationProvider migrationsAnnotations)
		: base(dependencies, migrationsAnnotations)
		{
		}

		protected override void Generate(MigrationOperation operation,IModel model,
			MigrationCommandListBuilder builder)
		{
			if (operation is ChangeIdentityOperation changeIdentityOperation)
			{
				Generate(changeIdentityOperation, builder);
			}
			else
			{
				base.Generate(operation, model, builder);
			}
		}
		private void Generate(MigrationOperation migrationOperation, MigrationCommandListBuilder builder)
		{
			var operation = migrationOperation as ChangeIdentityOperation;
			if (operation != null)
			{
				var tempPrincipalColumnName = "old_" + operation.PrincipalColumn;


				// 2. Drop the primary key constraint
				Generate(new DropPrimaryKeyOperation { Table = operation.PrincipalTable });


				// 4. Add the new primary key column with the new identity setting
				Generate(new AddColumnOperation(
					operation.PrincipalTable,
					new ColumnBuilder().Int(
						name: operation.PrincipalColumn,
						nullable: false,
						identity: operation.Change == IdentityChange.SwitchIdentityOn)));

				// 5. Update existing data so that previous foreign key relationships remain
				if (operation.Change != IdentityChange.SwitchIdentityOn)
				{
					// If the new column doesn’t have identity on then we can copy the old
					// values from the previous identity column
					Generate(new SqlOperation(
						"UPDATE " + operation.PrincipalTable +
						" SET " + operation.PrincipalColumn + " = " + tempPrincipalColumnName + ";"));
				}

				// 6. Drop old primary key column
				Generate(new DropColumnOperation(
					operation.PrincipalTable,
					tempPrincipalColumnName));

				// 7. Add primary key constraint
				Generate(new AddPrimaryKeyOperation
				{
					Table = operation.PrincipalTable,
					Columns = { operation.PrincipalColumn }
				});

			}
		}
	}
}
