namespace HR_ManagementSystem.Utilities
{
    public static  class Permission
    {
        public static List<string> GeneratePermissionsFromModule(string module)
        {
            return new List<string>
            {
                $"Permissions.{module}.View",
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }
        public static List<string> PermissionsList ()
        {
            var allPermissions = new List<string> ();
            foreach (var module in Enum.GetValues(typeof(PermissionModuleName)))
                allPermissions.AddRange(GeneratePermissionsFromModule(module.ToString()));
            return allPermissions;
        }
        public static class ApplicationUserPerm
        {
            public const string View = "Permissions.ApplicationUser.View";
            public const string Create = "Permissions.ApplicationUser.Create";
            public const string Edit = "Permissions.ApplicationUser.Edit";
            public const string Delete = "Permissions.ApplicationUser.Delete";
        }

        public static class Attendence
        {
            public const string View = "Permissions.Attendence.View";
            public const string Create = "Permissions.Attendence.Create";
            public const string Edit = "Permissions.Attendence.Edit";
            public const string Delete = "Permissions.Attendence.Delete";
        }

        public static class BasicUser
        {
            public const string View = "Permissions.BasicUser.View";
            public const string Create = "Permissions.BasicUser.Create";
            public const string Edit = "Permissions.BasicUser.Edit";
            public const string Delete = "Permissions.BasicUser.Delete";
        }

        public static class DaysOff
        {
            public const string View = "Permissions.DaysOff.View";
            public const string Create = "Permissions.DaysOff.Create";
            public const string Edit = "Permissions.DaysOff.Edit";
            public const string Delete = "Permissions.DaysOff.Delete";
        }

        public static class Department
        {
            public const string View = "Permissions.Department.View";
            public const string Create = "Permissions.Department.Create";
            public const string Edit = "Permissions.Department.Edit";
            public const string Delete = "Permissions.Department.Delete";
        }

        public static class Employee
        {
            public const string View = "Permissions.Employee.View";
            public const string Create = "Permissions.Employee.Create";
            public const string Edit = "Permissions.Employee.Edit";
            public const string Delete = "Permissions.Employee.Delete";
        }

        public static class Organization
        {
            public const string View = "Permissions.Organization.View";
            public const string Create = "Permissions.Organization.Create";
            public const string Edit = "Permissions.Organization.Edit";
            public const string Delete = "Permissions.Organization.Delete";
        }

        public static class Salary
        {
            public const string View = "Permissions.Salary.View";
            public const string Create = "Permissions.Salary.Create";
            public const string Edit = "Permissions.Salary.Edit";
            public const string Delete = "Permissions.Salary.Delete";
        }

        public static class SuperAdmin
        {
            public const string View = "Permissions.SuperAdmin.View";
            public const string Create = "Permissions.SuperAdmin.Create";
            public const string Edit = "Permissions.SuperAdmin.Edit";
            public const string Delete = "Permissions.SuperAdmin.Delete";
        }
    }
}
