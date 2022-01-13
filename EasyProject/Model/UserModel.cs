
namespace EasyProject.Model
{
    public class UserModel : Notifier
    {
        public string Nurse_no { get; set; }
        private string nurse_name;
        public string Nurse_name
        {
            get
            {
                return nurse_name;
            }
            set
            {
                nurse_name = value;
                OnPropertyChanged("Nurse_name");
            }
        }
        public string Nurse_auth { get; set; }
        public string Nurse_pw { get; set; }
        public int? Dept_id { get; set; }

        private string dept_name;
        public string Dept_name
        {
            get { return dept_name; }
            set
            {
                dept_name = value;
                OnPropertyChanged("Dept_name");
            }
        }
        private string dept_phone;
        public string Dept_phone 
        { 
            get { return dept_phone; }
            set
            {
                dept_phone = value;
                OnPropertyChanged("Dept_phone");
            }
        }
        public string Dept_status { get; set; }

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

    }//class

}//namespace
