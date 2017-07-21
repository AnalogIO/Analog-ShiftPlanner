namespace Data.MSSQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialmigrationformssql : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CheckIns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Employee_Id = c.Int(),
                        Shift_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Employee_Id)
                .ForeignKey("dbo.Shifts", t => t.Shift_Id)
                .Index(t => t.Employee_Id)
                .Index(t => t.Shift_Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Password = c.String(),
                        Salt = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Active = c.Boolean(nullable: false),
                        Organization_Id = c.Int(),
                        EmployeeTitle_Id = c.Int(),
                        Photo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .ForeignKey("dbo.EmployeeTitles", t => t.EmployeeTitle_Id)
                .ForeignKey("dbo.Photos", t => t.Photo_Id)
                .Index(t => t.Organization_Id)
                .Index(t => t.EmployeeTitle_Id)
                .Index(t => t.Photo_Id);
            
            CreateTable(
                "dbo.EmployeeTitles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ShortKey = c.String(),
                        ApiKey = c.String(),
                        DefaultPhoto_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Photos", t => t.DefaultPhoto_Id)
                .Index(t => t.DefaultPhoto_Id);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.Binary(),
                        Type = c.String(),
                        Organization_Id = c.Int(),
                        Organization_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id1)
                .Index(t => t.Organization_Id)
                .Index(t => t.Organization_Id1);
            
            CreateTable(
                "dbo.Managers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Salt = c.String(),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TokenHash = c.String(),
                        Manager_Id = c.Int(),
                        Employee_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Managers", t => t.Manager_Id)
                .ForeignKey("dbo.Employees", t => t.Employee_Id)
                .Index(t => t.Manager_Id)
                .Index(t => t.Employee_Id);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NumberOfWeeks = c.Int(nullable: false),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "dbo.ScheduledShifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Day = c.Int(nullable: false),
                        Start = c.Time(nullable: false, precision: 7),
                        End = c.Time(nullable: false, precision: 7),
                        Schedule_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schedules", t => t.Schedule_Id)
                .Index(t => t.Schedule_Id);
            
            CreateTable(
                "dbo.Shifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Organization_Id = c.Int(),
                        Schedule_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .ForeignKey("dbo.Schedules", t => t.Schedule_Id)
                .Index(t => t.Organization_Id)
                .Index(t => t.Schedule_Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Employee_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Employee_Id)
                .Index(t => t.Employee_Id);
            
            CreateTable(
                "dbo.ScheduledShiftEmployees",
                c => new
                    {
                        ScheduledShift_Id = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ScheduledShift_Id, t.Employee_Id })
                .ForeignKey("dbo.ScheduledShifts", t => t.ScheduledShift_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.ScheduledShift_Id)
                .Index(t => t.Employee_Id);
            
            CreateTable(
                "dbo.ShiftEmployees",
                c => new
                    {
                        Shift_Id = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Shift_Id, t.Employee_Id })
                .ForeignKey("dbo.Shifts", t => t.Shift_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Shift_Id)
                .Index(t => t.Employee_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tokens", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.Roles", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.Employees", "Photo_Id", "dbo.Photos");
            DropForeignKey("dbo.Employees", "EmployeeTitle_Id", "dbo.EmployeeTitles");
            DropForeignKey("dbo.Shifts", "Schedule_Id", "dbo.Schedules");
            DropForeignKey("dbo.Shifts", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.ShiftEmployees", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.ShiftEmployees", "Shift_Id", "dbo.Shifts");
            DropForeignKey("dbo.CheckIns", "Shift_Id", "dbo.Shifts");
            DropForeignKey("dbo.ScheduledShifts", "Schedule_Id", "dbo.Schedules");
            DropForeignKey("dbo.ScheduledShiftEmployees", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.ScheduledShiftEmployees", "ScheduledShift_Id", "dbo.ScheduledShifts");
            DropForeignKey("dbo.Schedules", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Photos", "Organization_Id1", "dbo.Organizations");
            DropForeignKey("dbo.Tokens", "Manager_Id", "dbo.Managers");
            DropForeignKey("dbo.Managers", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.EmployeeTitles", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Employees", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Organizations", "DefaultPhoto_Id", "dbo.Photos");
            DropForeignKey("dbo.Photos", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.CheckIns", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.ShiftEmployees", new[] { "Employee_Id" });
            DropIndex("dbo.ShiftEmployees", new[] { "Shift_Id" });
            DropIndex("dbo.ScheduledShiftEmployees", new[] { "Employee_Id" });
            DropIndex("dbo.ScheduledShiftEmployees", new[] { "ScheduledShift_Id" });
            DropIndex("dbo.Roles", new[] { "Employee_Id" });
            DropIndex("dbo.Shifts", new[] { "Schedule_Id" });
            DropIndex("dbo.Shifts", new[] { "Organization_Id" });
            DropIndex("dbo.ScheduledShifts", new[] { "Schedule_Id" });
            DropIndex("dbo.Schedules", new[] { "Organization_Id" });
            DropIndex("dbo.Tokens", new[] { "Employee_Id" });
            DropIndex("dbo.Tokens", new[] { "Manager_Id" });
            DropIndex("dbo.Managers", new[] { "Organization_Id" });
            DropIndex("dbo.Photos", new[] { "Organization_Id1" });
            DropIndex("dbo.Photos", new[] { "Organization_Id" });
            DropIndex("dbo.Organizations", new[] { "DefaultPhoto_Id" });
            DropIndex("dbo.EmployeeTitles", new[] { "Organization_Id" });
            DropIndex("dbo.Employees", new[] { "Photo_Id" });
            DropIndex("dbo.Employees", new[] { "EmployeeTitle_Id" });
            DropIndex("dbo.Employees", new[] { "Organization_Id" });
            DropIndex("dbo.CheckIns", new[] { "Shift_Id" });
            DropIndex("dbo.CheckIns", new[] { "Employee_Id" });
            DropTable("dbo.ShiftEmployees");
            DropTable("dbo.ScheduledShiftEmployees");
            DropTable("dbo.Roles");
            DropTable("dbo.Shifts");
            DropTable("dbo.ScheduledShifts");
            DropTable("dbo.Schedules");
            DropTable("dbo.Tokens");
            DropTable("dbo.Managers");
            DropTable("dbo.Photos");
            DropTable("dbo.Organizations");
            DropTable("dbo.EmployeeTitles");
            DropTable("dbo.Employees");
            DropTable("dbo.CheckIns");
        }
    }
}
