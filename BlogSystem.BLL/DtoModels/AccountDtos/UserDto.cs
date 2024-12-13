namespace BlogSystem.BLL.DtoModels.AccountDtos
{
    public class UserDto
    {

        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string? Token { get; set; }
        public List<string>? Errors { get; set; }
    }
}
