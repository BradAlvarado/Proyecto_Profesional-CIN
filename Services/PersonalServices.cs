using Sistema_CIN.Models;

namespace Sistema_CIN.Services
{
    public class PersonalServices
    {
        private static Personal _personalModel;
        public static void AddPersonal(Personal personalModel)
        {
            _personalModel = personalModel;
        }

        public static Personal GetPersonal()
        {
            return _personalModel;
        }
    }
}
