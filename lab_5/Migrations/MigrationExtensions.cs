using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations;

namespace lab_5.Migrations
{
	public static class MigrationExtensions
	{
		public static MigrationBuilder ChangeIdentity(
			this MigrationBuilder migrationBuilder,
			IdentityChange change,
			string principalTable,
			string principalColumn)
		{


			migrationBuilder.Operations.Add(
			   new ChangeIdentityOperation
			   {
				   Change = change,
				   PrincipalTable = principalTable,
				   PrincipalColumn = principalColumn,
				   DependentColumns = new List<DependentColumn>()
			   });



			return migrationBuilder;
		}
	}
}
