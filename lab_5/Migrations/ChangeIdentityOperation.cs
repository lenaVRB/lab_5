﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity.Migrations.Model;

namespace lab_5.Migrations
{
	public class ChangeIdentityOperation : MigrationOperation
	{
		public ChangeIdentityOperation()
			: base(null)
		{ }

		public IdentityChange Change { get; set; }
		public string PrincipalTable { get; set; }
		public string PrincipalColumn { get; set; }
		public List<DependentColumn> DependentColumns { get; set; }

		public override bool IsDestructiveChange
		{
			get { return false; }
		}
	}

	public enum IdentityChange
	{
		SwitchIdentityOn,
		SwitchIdentityOff
	}

	public class DependentColumn
	{
		public string DependentTable { get; set; }
		public string ForeignKeyColumn { get; set; }
	}
}
