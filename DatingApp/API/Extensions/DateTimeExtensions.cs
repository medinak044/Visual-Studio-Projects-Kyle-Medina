namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) --age; // Decrement age because the user hasn't had their birthday yet this year
            return age;
        }
    }
}
