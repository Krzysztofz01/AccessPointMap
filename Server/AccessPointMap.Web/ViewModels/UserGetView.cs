namespace AccessPointMap.Web.ViewModels
{
    public class UserGetView
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool AdminPermission { get; set; }
        public bool ModPermission { get; set; }
        public bool IsActivated { get; set; }
    }
}
