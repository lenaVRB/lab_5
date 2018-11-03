using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace lab_5.Migrations
{
	public class ChangeIdentityOperation : MigrationOperation
	{
		public ChangeIdentityOperation() : base()
		{
		}
			
		public IdentityChange Change { get; set; }
		public string PrincipalTable { get; set; }
		public string PrincipalColumn { get; set; }

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
}
