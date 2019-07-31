using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Win32;
using System.Drawing;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public class Users
    {
        public BitmapImage ImageUrl { get; set; }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Sunday_start { get; set; }
        public string Monday_start { get; set; }
        public string Tuesday_start { get; set; }
        public string Wednesday_start { get; set; }
        public string Thursday_start { get; set; }
        public string Friday_start { get; set; }
        public string Sunday_end { get; set; }
        public string Monday_end { get; set; }
        public string Tuesday_end { get; set; }
        public string Wednesday_end { get; set; }
        public string Thursday_end { get; set; }
        public string Friday_end { get; set; }
    }


    public partial class MainWindow : Window
    {
        public void FillComboBoxAndShowEmployeeDetail()
        {
            int j = 0;
            IdSelectUpdate.Items.Clear();
            IdSelectDelete.Items.Clear();
            HomeEmployeeDetail.Items.Clear();

            string connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            MySqlConnection connection = new MySqlConnection(connectionstring);
            MySqlCommand cmd = new MySqlCommand("select image, concat(first_name,' ',last_name) as name, id from employeedatabase.employeedata", connection);
            connection.Open();
            MySqlDataReader empReader = cmd.ExecuteReader();

            while (empReader.Read())
            {
                IdSelectUpdate.Items.Add(empReader["Id"]);
                IdSelectDelete.Items.Add(empReader["Id"]);
                HomeEmployeeDetail.Items.Add(new compactUserDetail(Convert.ToInt32(empReader["id"]), j++));

            }

            //close the mysql connection
            connection.Close();

            IdSelectUpdate.SelectedIndex = 0;
            IdSelectDelete.SelectedIndex = 0;
            fetchFrontPage();
        }

        public MainWindow()
        {
            InitializeComponent();
            FillComboBoxAndShowEmployeeDetail();
        }


        public void FrontPageDisplay(int[] schedule)
        {
            string[] shifts = { "First Shift", "Second Shift", "Third Shift", "Fourth Shift", "Fifth Shift", "Sixth Shift", "Seventh Shift", "Eighth Shift", "Ninth Shift", "Tenth Shift", "Eleventh", "Twelveth", "Thirtheenth", "Fourteenth", "Fifteenth", "Sixteenth", "Seventeenth", "Eighteenth", "Nineteenth", "Twentieth", "Twenty First", "Twenty Second", "Twenty Third", "Twenty Fourth" };
            HomeDisplay.Children.Clear();
            ShiftHome.Children.Clear();       //not sure if this is needed

            string connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            MySqlConnection connection;
            MySqlCommand cmd;
            List<string> res = new List<string>();

            for (int j = 0; j < schedule.Length; ++j)
            {
                connection = new MySqlConnection(connectionstring);
                cmd = new MySqlCommand("select first_name,last_name,id,gender,phone,email from employeedatabase.employeedata where id='" + schedule[j] + "'", connection);
                connection.Open();
                MySqlDataReader empReader = cmd.ExecuteReader();
                while (empReader.Read())
                    res.Add(empReader["first_name"].ToString());
                connection.Close();
            }

            //display the names in the ui
            for (int j = 0; j < Globals.WORKING_HOUR; ++j)
            {
                HomeDisplay.Children.Add(new HomeScheduleDisplay(new TimeSchedule(res[j], j,schedule[j]), j));
                ShiftHome.Children.Add(new HomePageDetail(shifts[j], j));
            }
        }

        public void Display(int[] schedule, int i)
        {
            string[] shifts = { "First Shift", "Second Shift", "Third Shift", "Fourth Shift", "Fifth Shift", "Sixth Shift", "Seventh Shift", "Eighth Shift", "Ninth Shift", "Tenth Shift", "Eleventh", "Twelveth", "Thirtheenth", "Fourteenth", "Fifteenth", "Sixteenth", "Seventeenth", "Eighteenth", "Nineteenth", "Twentieth", "Twenty First", "Twenty Second", "Twenty Third", "Twenty Fourth" };
            ShiftLabels.Children.Clear();
            //empty the tab if new schedule is to be displayed
            if (i == 0)
                Container.Children.Clear();

            //get names by using the id from the database

            string connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            MySqlConnection connection;
            MySqlCommand cmd;

            List<string> res = new List<string>();

            for (int j = 0; j < schedule.Length; ++j)
            {
                connection = new MySqlConnection(connectionstring);
                cmd = new MySqlCommand("select first_name,last_name,id,gender,phone,email from employeedatabase.employeedata where id='" + schedule[j] + "'", connection);
                connection.Open();
                MySqlDataReader empReader = cmd.ExecuteReader();
                while (empReader.Read())
                    res.Add(empReader["first_name"].ToString());
                connection.Close();
            }




            //display the names in the ui
            for (int j = 0; j < Globals.WORKING_HOUR; ++j)
            {
                Container.Children.Add(new ScheduleDisplay(new TimeSchedule(res[j], j, schedule[j]), i, j));
                ShiftLabels.Children.Add(new scheduleHeader(shifts[j], j));
            }
        }

        private void addDetBtn_Click(object sender, RoutedEventArgs e)
        {

            string error = "";
            error += ErrorMessageForTimings("Sunday", Convert.ToInt32(sundayStart.Text), Convert.ToInt32(sundayEnd.Text));
            error += ErrorMessageForTimings("Monday", Convert.ToInt32(mondayStart.Text), Convert.ToInt32(mondayEnd.Text));
            error += ErrorMessageForTimings("Tuesday", Convert.ToInt32(tuesdayStart.Text), Convert.ToInt32(tuesdayEnd.Text));
            error += ErrorMessageForTimings("Wednesday", Convert.ToInt32(wednesdayStart.Text), Convert.ToInt32(wednesdayEnd.Text));
            error += ErrorMessageForTimings("Thursday", Convert.ToInt32(thursdayStart.Text), Convert.ToInt32(thursdayEnd.Text));
            error += ErrorMessageForTimings("Friday", Convert.ToInt32(fridayStart.Text), Convert.ToInt32(fridayEnd.Text));
            if (!(error == ""))
            {
                MessageBox.Show(error, "Error");
                return;
            }


            byte[] ImageData;
            FileStream fs = new FileStream(this.path.Text, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            ImageData = br.ReadBytes((int)fs.Length);

            string connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            MySqlConnection connection = new MySqlConnection(connectionstring);
            MySqlCommand cmd = new MySqlCommand("insert into employeedata(id,First_Name,Last_Name,Gender,Phone,Email,Sunday_Start,Sunday_End,Monday_Start,Monday_End,Tuesday_Start,Tuesday_End,Wednesday_Start,Wednesday_End,thursday_Start,thursday_End,Friday_Start,Friday_End,Image) values('" + this.employeeIdInput.Text + "','" + this.employeeNameInput.Text + "','" + this.employeeLastNameInput.Text + "','" + this.employeeGenderInput.Text + "','" + this.employeePhoneInput.Text + "','" + this.employeeEmailInput.Text + "','" + this.sundayStart.Text + "','" + this.sundayEnd.Text + "','" + this.mondayStart.Text + "','" + this.mondayEnd.Text + "','" + this.tuesdayStart.Text + "','" + this.tuesdayEnd.Text + "','" + this.wednesdayStart.Text + "','" + this.wednesdayEnd.Text + "','" + this.thursdayStart.Text + "','" + this.thursdayEnd.Text + "','" + this.fridayStart.Text + "','" + this.fridayEnd.Text + "',@image); ", connection);
            MySqlDataReader myReader;
            DataTable dt = new DataTable();
            try
            {
                connection.Open();
                cmd.Parameters.Add(new MySqlParameter("@image", ImageData));
                myReader = cmd.ExecuteReader();


                while (myReader.Read())
                {

                }

                
            MessageBox.Show("Added new employee!");

            }
            catch (Exception ex)

            {

                MessageBox.Show(ex.Message);

            }

            empImage.Source = null;
            employeeIdInput.Clear();
            employeeNameInput.Clear();
            employeeLastNameInput.Clear();
            employeePhoneInput.Clear();
            employeeEmailInput.Clear();

            sundayStartUpdate.Text = "0";
            mondayStartUpdate.Text = "0";
            tuesdayStartUpdate.Text = "0";
            wednesdayStartUpdate.Text = "0";
            thursdayStartUpdate.Text = "0";
            fridayStartUpdate.Text = "0";
            sundayEndUpdate.Text = "24";
            mondayEndUpdate.Text = "24";
            tuesdayEndUpdate.Text = "24";
            wednesdayEndUpdate.Text = "24";
            thursdayEndUpdate.Text = "24";
            fridayEndUpdate.Text = "24";

            FillComboBoxAndShowEmployeeDetail();

            // FileStream f = new FileStream("details.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None)
        }


        private void updateDetBtn_Click(object sender, RoutedEventArgs e)
        {
            string error = "";
            error += ErrorMessageForTimings("Sunday", Convert.ToInt32(sundayStartUpdate.Text), Convert.ToInt32(sundayEndUpdate.Text));
            error += ErrorMessageForTimings("Monday", Convert.ToInt32(mondayStartUpdate.Text), Convert.ToInt32(mondayEndUpdate.Text));
            error += ErrorMessageForTimings("Tuesday", Convert.ToInt32(tuesdayStartUpdate.Text), Convert.ToInt32(tuesdayEndUpdate.Text));
            error += ErrorMessageForTimings("Wednesday", Convert.ToInt32(wednesdayStartUpdate.Text), Convert.ToInt32(wednesdayEndUpdate.Text));
            error += ErrorMessageForTimings("Thursday", Convert.ToInt32(thursdayStartUpdate.Text), Convert.ToInt32(thursdayEndUpdate.Text));
            error += ErrorMessageForTimings("Friday", Convert.ToInt32(fridayStartUpdate.Text), Convert.ToInt32(fridayEndUpdate.Text));
            if (!(error == ""))
            {
                MessageBox.Show(error, "Error");
                return;
            }

            byte[] ImageDt;
            FileStream fs = new FileStream(this.path1.Text, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            ImageDt = br.ReadBytes((int)fs.Length);

            string connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            MySqlConnection connection = new MySqlConnection(connectionstring);
            MySqlCommand cmd = new MySqlCommand("update employeedatabase.employeedata set First_Name='" + this.employeeNameUpdate.Text + "',Last_Name='" + this.employeeLastNameUpdate.Text + "',Gender='" + this.employeeGenderUpdate.Text + "',Phone='" + this.employeePhoneUpdate.Text + "',Email='" + this.employeeEmailUpdate.Text + "',Sunday_Start='" + this.sundayStartUpdate.Text + "',Sunday_End='" + this.sundayEndUpdate.Text + "',Monday_Start='" + this.mondayStartUpdate.Text + "',Monday_End='" + this.mondayEndUpdate.Text + "',Tuesday_Start='" + this.tuesdayStartUpdate.Text + "',Tuesday_End='" + this.tuesdayEndUpdate.Text + "',Wednesday_Start='" + this.wednesdayStartUpdate.Text + "',Wednesday_End='" + this.wednesdayEndUpdate.Text + "',thursday_Start='" + this.thursdayStartUpdate.Text + "',thursday_End='" + this.thursdayEndUpdate.Text + "',Friday_Start='" + this.fridayStartUpdate.Text + "',Friday_End='" + this.fridayEndUpdate.Text + "',Image=@image where ID='" + IdSelectUpdate.SelectedItem + "'; ", connection);
            MySqlDataReader myReader;
            DataTable dt = new DataTable();

            try
            {
                connection.Open();
                cmd.Parameters.Add(new MySqlParameter("@image", ImageDt));
                myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }

            employeeNameUpdate.Clear();
            employeeLastNameUpdate.Clear();

            employeePhoneUpdate.Clear();
            employeeEmailUpdate.Clear();
            sundayStartUpdate.Clear();
            mondayStartUpdate.Clear();
            tuesdayStartUpdate.Clear();
            wednesdayStartUpdate.Clear();
            thursdayStartUpdate.Clear();
            fridayStartUpdate.Clear();
            sundayEndUpdate.Clear();
            mondayEndUpdate.Clear();
            tuesdayEndUpdate.Clear();
            wednesdayEndUpdate.Clear();
            thursdayEndUpdate.Clear();
            fridayEndUpdate.Clear();

            FillComboBoxAndShowEmployeeDetail();
            MessageBox.Show("Update the info!");

        }

        private void deleteDetBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this Employee Info", "Roster Manager", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:

                    string connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
                    MySqlConnection connection = new MySqlConnection(connectionstring);
                    MySqlCommand cmd = new MySqlCommand("delete from employeedatabase.employeedata where ID = '" + this.IdSelectDelete.Text + "'; ", connection);
                    connection.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    connection.Close();
                    nameDelete.Clear();
                    genderDelete.Clear();
                    phoneDelete.Clear();
                    emailDelete.Clear();
                    delTabImage.Source = null;

                    FillComboBoxAndShowEmployeeDetail();

                    MessageBox.Show("Deleted employee info!");
                    break;

                case MessageBoxResult.No:
                    MessageBox.Show("Employee Info was not Deleted!!", "Roster Manager");
                    break;
            }
        }

        private void dispBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Users> detGridDetails = new List<Users>();

            string connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            MySqlConnection connection = new MySqlConnection(connectionstring);
            MySqlCommand cmd = new MySqlCommand("select * from employeedatabase.employeedata", connection);
            connection.Open();

            MySqlDataReader empReader = cmd.ExecuteReader();
            try
            {
                while (empReader.Read())
                {
                    BitmapImage temp = new BitmapImage();


                    byte[] data = (byte[])(empReader["Image"]);

                    if (data == null)
                    {
                        temp = null;
                    }
                    else
                    {
                        MemoryStream strm = new MemoryStream();
                        strm.Write(data, 0, data.Length);
                        strm.Position = 0;
                        System.Drawing.Image img = System.Drawing.Image.FromStream(strm);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        temp = bi;
                    }
                    detGridDetails.Add(new Users() { Id = Convert.ToInt32(empReader["ID"]), FirstName = empReader["First_name"].ToString(), LastName = empReader["Last_name"].ToString(), Gender = empReader["gender"].ToString(), PhoneNo = empReader["phone"].ToString(), Email = empReader["email"].ToString(), Sunday_start = empReader["sunday_start"].ToString(), Monday_start = empReader["monday_start"].ToString(), Tuesday_start = empReader["tuesday_start"].ToString(), Wednesday_start = empReader["wednesday_start"].ToString(), Thursday_start = empReader["thursday_start"].ToString(), Friday_start = empReader["friday_start"].ToString(), Sunday_end = empReader["sunday_end"].ToString(), Monday_end = empReader["monday_end"].ToString(), Tuesday_end = empReader["tuesday_end"].ToString(), Wednesday_end = empReader["wednesday_end"].ToString(), Thursday_end = empReader["thursday_end"].ToString(), Friday_end = empReader["friday_end"].ToString(), ImageUrl = temp });
                }

            }
            catch (Exception)
            {

            }
            detGrid.ItemsSource = detGridDetails;
            connection.Close();
        }

        private void generateBtn_Click(object sender, RoutedEventArgs e)
        {
            //ITERATION 1

            string connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            MySqlConnection connection = new MySqlConnection(connectionstring);
            MySqlCommand cmd = new MySqlCommand("select * from employeedatabase.employeedata", connection);
            connection.Open();
            MySqlDataReader empReader = cmd.ExecuteReader();

            /**************************************
             * the commented code is for file i/o*
             *************************************/

            int[] ReturnedSchedule = new int[Globals.WORKING_HOUR];
            //StreamReader s = new StreamReader(@"C:\users\user\documents\EmployeeDetails.txt");
            //string ln = "";
            //while ((ln = s.ReadLine()) != null)
            while (empReader.Read())
            {
                //string[] wrds = ln.Split();
                Employee emp = new Employee(empReader["First_Name"].ToString(), Convert.ToInt32(empReader["ID"]), Convert.ToInt32(empReader["Sunday_start"]), Convert.ToInt32(empReader["sunday_end"]));
                Globals.domain.Add(emp);
            }

            //close the mysql connection
            connection.Close();
            ReturnedSchedule = Program.generateSchedule();
            //List<User> users = new List<User>();
            //users.Add(new User() { Name = "Sunday", a = 'A', b = 'A', c = 'C', d = 'C', e = 'C', f = 'E', g = 'E', h = 'E' });
            //detGrid.ItemsSource = users;

            //update the schedule in database
            string queryString;
            string[] queryList = new string[24];
            int m;
            for (m = 0; m < ReturnedSchedule.Length; ++m)
            {
                queryList[m] = (ReturnedSchedule[m].ToString());
            }

            for (; m < 24; ++m)
            {
                queryList[m] = ("NULL");
            }

            queryString = string.Join(",", queryList);


            connection = new MySqlConnection(connectionstring);
            cmd = new MySqlCommand("replace into employeeshift values ('Sunday'," + queryString + ")", connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            //updated in the database at this point

            //display the schedule in the ui


            //ITERATION 2

            connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            connection = new MySqlConnection(connectionstring);
            cmd = new MySqlCommand("select * from employeedatabase.employeedata", connection);
            connection.Open();
            empReader = cmd.ExecuteReader();

            while (empReader.Read())
            {
                Employee emp = new Employee(empReader["First_Name"].ToString(), Convert.ToInt32(empReader["ID"]), Convert.ToInt32(empReader["monday_start"]), Convert.ToInt32(empReader["monday_end"]));
                Globals.domain.Add(emp);
            }
            connection.Close();
            ReturnedSchedule = Program.generateSchedule();

            //update the schedule in database
            for (m = 0; m < ReturnedSchedule.Length; ++m)
            {
                queryList[m] = (ReturnedSchedule[m].ToString());
            }

            for (; m < 24; ++m)
            {
                queryList[m] = ("NULL");
            }

            queryString = string.Join(",", queryList);


            connection = new MySqlConnection(connectionstring);
            cmd = new MySqlCommand("replace into employeeshift values ('Monday'," + queryString + ")", connection);
            connection.Open();
            dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();


            //ITERATION 3

            connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            connection = new MySqlConnection(connectionstring);
            cmd = new MySqlCommand("select * from employeedatabase.employeedata", connection);
            connection.Open();
            empReader = cmd.ExecuteReader();

            while (empReader.Read())
            {
                Employee emp = new Employee(empReader["First_Name"].ToString(), Convert.ToInt32(empReader["ID"]), Convert.ToInt32(empReader["tuesday_start"]), Convert.ToInt32(empReader["tuesday_end"]));
                Globals.domain.Add(emp);
            }
            connection.Close();
            ReturnedSchedule = Program.generateSchedule();

            //update the schedule in database
            for (m = 0; m < ReturnedSchedule.Length; ++m)
            {
                queryList[m] = (ReturnedSchedule[m].ToString());
            }

            for (; m < 24; ++m)
            {
                queryList[m] = ("NULL");
            }

            queryString = string.Join(",", queryList);


            connection = new MySqlConnection(connectionstring);
            cmd = new MySqlCommand("replace into employeeshift values ('Tuesday'," + queryString + ")", connection);
            connection.Open();
            dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            //updated in the database at this point



            //ITERATION 4

            connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            connection = new MySqlConnection(connectionstring);
            cmd = new MySqlCommand("select * from employeedatabase.employeedata", connection);
            connection.Open();
            empReader = cmd.ExecuteReader();

            while (empReader.Read())
            {
                Employee emp = new Employee(empReader["First_Name"].ToString(), Convert.ToInt32(empReader["ID"]), Convert.ToInt32(empReader["wednesday_start"]), Convert.ToInt32(empReader["wednesday_end"]));
                Globals.domain.Add(emp);
            }
            connection.Close();
            ReturnedSchedule = Program.generateSchedule();

            //update the schedule in database
            for (m = 0; m < ReturnedSchedule.Length; ++m)
            {
                queryList[m] = (ReturnedSchedule[m].ToString());
            }

            for (; m < 24; ++m)
            {
                queryList[m] = ("NULL");
            }

            queryString = string.Join(",", queryList);


            connection = new MySqlConnection(connectionstring);
            cmd = new MySqlCommand("replace into employeeshift values ('Wednesday'," + queryString + ")", connection);
            connection.Open();
            dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            //updated in the database at this point




            //ITERATION 5

            connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            connection = new MySqlConnection(connectionstring);
            cmd = new MySqlCommand("select * from employeedatabase.employeedata", connection);
            connection.Open();
            empReader = cmd.ExecuteReader();

            while (empReader.Read())
            {
                Employee emp = new Employee(empReader["First_Name"].ToString(), Convert.ToInt32(empReader["ID"]), Convert.ToInt32(empReader["thursday_start"]), Convert.ToInt32(empReader["thursday_end"]));
                Globals.domain.Add(emp);
            }
            connection.Close();
            ReturnedSchedule = Program.generateSchedule();

            //update the schedule in database
            for (m = 0; m < ReturnedSchedule.Length; ++m)
            {
                queryList[m] = (ReturnedSchedule[m].ToString());
            }

            for (; m < 24; ++m)
            {
                queryList[m] = ("NULL");
            }

            queryString = string.Join(",", queryList);


            connection = new MySqlConnection(connectionstring);
            cmd = new MySqlCommand("replace into employeeshift values ('thursday'," + queryString + ")", connection);
            connection.Open();
            dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            //updated in the database at this point


            //ITERATION 6

            connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            connection = new MySqlConnection(connectionstring);
            cmd = new MySqlCommand("select * from employeedatabase.employeedata", connection);
            connection.Open();
            empReader = cmd.ExecuteReader();

            while (empReader.Read())
            {
                Employee emp = new Employee(empReader["First_Name"].ToString(), Convert.ToInt32(empReader["ID"]), Convert.ToInt32(empReader["friday_start"]), Convert.ToInt32(empReader["friday_end"]));
                Globals.domain.Add(emp);
            }
            connection.Close();
            ReturnedSchedule = Program.generateSchedule();

            //update the schedule in database
            for (m = 0; m < ReturnedSchedule.Length; ++m)
            {
                queryList[m] = (ReturnedSchedule[m].ToString());
            }

            for (; m < 24; ++m)
            {
                queryList[m] = ("NULL");
            }

            queryString = string.Join(",", queryList);


            connection = new MySqlConnection(connectionstring);
            cmd = new MySqlCommand("replace into employeeshift values ('Friday'," + queryString + ")", connection);
            connection.Open();
            dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            //updated in the database at this point

            fetch();


        }

        private void constraintBtn_Click(object sender, RoutedEventArgs e)
        {
            Globals.atleastTwo = (firstConstraint.IsChecked == true);
            Globals.nonRepetitive = (secondConstraint.IsChecked == true);
            Globals.empTimeConsideration = (thirdConstraint.IsChecked == true);
            Globals.maxWorkPerDay = (fourthConstraint.IsChecked == true);


            Globals.STARTING_HOUR = Convert.ToInt32(startTimeInput.Text);
            Globals.ENDING_HOUR = Convert.ToInt32(endTimeInput.Text);
            Globals.WORKING_HOUR = Globals.ENDING_HOUR - Globals.STARTING_HOUR;
            Globals.maxWorkHourPerDay = Convert.ToInt32(maxDayInput.Text);
            Globals.maxWorkHourPerWeek = Convert.ToInt32(maxWeekInput.Text);

            MessageBox.Show("Constraints are Set!");
        }

        private void lessthan24Input_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int i;
            string str = (((TextBox)sender).Text + e.Text);
            e.Handled = !(int.TryParse(str, out i) && i >= 0 && i <= 24);
        }

        private void minWeekInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int i;
            string str = (((TextBox)sender).Text + e.Text);
            e.Handled = !(int.TryParse(str, out i) && i >= 0 && i <= 120);
        }

        private void maxWeekInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int i;
            string str = (((TextBox)sender).Text + e.Text);
            e.Handled = !(int.TryParse(str, out i) && i >= 0 && i <= 168);
        }

        private void isAlphabet_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z]"))
            {
                e.Handled = true;
            }
        }

        private void isPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            double i;
            string str = (((TextBox)sender).Text + e.Text);
            e.Handled = !(double.TryParse(str, out i) && i > 0 && i < 10000000000);
        }

        private void endText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void minContinuousInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void startTimeInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void endTimeInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



        private void maxWeekInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void minWeekInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void maxDayInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void detGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void findBtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            MySqlConnection connection = new MySqlConnection(connectionstring);
            MySqlCommand cmd = new MySqlCommand("select * from employeedatabase.employeedata where Id=" + IdSelectUpdate.SelectedItem, connection);
            connection.Open();

            MySqlDataReader empReader = cmd.ExecuteReader();
            try
            {
                while (empReader.Read())
                {

                    employeeNameUpdate.Text = empReader["First_name"].ToString();
                    employeeLastNameUpdate.Text = empReader["Last_Name"].ToString();
                    employeeGenderUpdate.Text = empReader["Gender"].ToString();
                    employeePhoneUpdate.Text = empReader["Phone"].ToString();
                    employeeEmailUpdate.Text = empReader["Email"].ToString();
                    sundayStartUpdate.Text = empReader["Sunday_Start"].ToString();
                    sundayEndUpdate.Text = empReader["Sunday_End"].ToString();
                    mondayStartUpdate.Text = empReader["Monday_Start"].ToString();
                    mondayEndUpdate.Text = empReader["Monday_End"].ToString();
                    tuesdayStartUpdate.Text = empReader["Tuesday_Start"].ToString();
                    tuesdayEndUpdate.Text = empReader["Tuesday_End"].ToString();
                    wednesdayStartUpdate.Text = empReader["Wednesday_Start"].ToString();
                    wednesdayEndUpdate.Text = empReader["Wednesday_End"].ToString();
                    thursdayStartUpdate.Text = empReader["Thursday_Start"].ToString();
                    thursdayEndUpdate.Text = empReader["Thursday_End"].ToString();
                    fridayStartUpdate.Text = empReader["Friday_Start"].ToString();
                    fridayEndUpdate.Text = empReader["Friday_End"].ToString();

                    byte[] data = (byte[])(empReader["Image"]);

                    if (data == null)
                    {
                        updatedImage.Source = null;
                    }
                    else
                    {

                        MemoryStream strm = new MemoryStream();
                        strm.Write(data, 0, data.Length);
                        strm.Position = 0;
                        System.Drawing.Image img = System.Drawing.Image.FromStream(strm);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        updatedImage.Source = bi;
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        private void findBtnDelete_Click(object sender, RoutedEventArgs e)
        {
            string connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            MySqlConnection connection = new MySqlConnection(connectionstring);
            MySqlCommand cmd = new MySqlCommand("select * from employeedatabase.employeedata where Id=" + IdSelectDelete.SelectedItem, connection);
            connection.Open();

            MySqlDataReader empReader = cmd.ExecuteReader();
            try
            {
                while (empReader.Read())
                {

                    nameDelete.Text = empReader["First_name"].ToString() + " " + empReader["Last_Name"].ToString();
                    genderDelete.Text = empReader["Gender"].ToString();
                    phoneDelete.Text = empReader["Phone"].ToString();
                    emailDelete.Text = empReader["Email"].ToString();


                    byte[] data = (byte[])(empReader["Image"]);

                    if (data == null)
                    {
                        updatedImage.Source = null;
                    }
                    else
                    {

                        MemoryStream strm = new MemoryStream();
                        strm.Write(data, 0, data.Length);
                        strm.Position = 0;
                        System.Drawing.Image img = System.Drawing.Image.FromStream(strm);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        delTabImage.Source = bi;
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        private void HomeIconBtn_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.SelectedItem = homeTab;
        }

        private void AddIconBtn_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.SelectedItem = addTab;
        }

        private void EditIconBtn_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.SelectedItem = editTab;
        }

        private void DeleteIconBtn_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.SelectedItem = deleteTab;
        }

        private void HomeLogo_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.SelectedItem = homeTab;
        }

        private string ErrorMessageForTimings(string day, int start, int end)
        {
            string error = "";
            if (start > end)
            {
                error += "The starting time for " + day + " is greater than ending time for that day \n";
            }
            return error;
        }


        public void suggestionDisplay(int gridRow, int gridColumn)
        {
            suggestionStack.Children.Clear();
            suggestionStack.Children.Add(new suggestion("Suggestion", 0, 0, 0, 0, 0));

            string[] days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            int time = (gridColumn + Globals.STARTING_HOUR);

            int displayCount = 0;

            string connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            MySqlConnection connection = new MySqlConnection(connectionstring);
            MySqlCommand cmd = new MySqlCommand("select * from employeedatabase.employeedata where " + days[gridRow] + "_Start <=" + time + " and " + days[gridRow] + "_End >" + time + ";", connection);
            connection.Open();

            MySqlDataReader empReader = cmd.ExecuteReader();
            while (empReader.Read())
            {
                int displayRow = (displayCount) / (days.Count());
                int displayColumn = displayCount % days.Count() + 1;
                suggestionStack.Children.Add(new suggestion(empReader["First_Name"].ToString(), Convert.ToInt32(empReader["Id"]), displayRow, displayColumn, gridRow, gridColumn));
                displayCount++;
            }

            connection.Close();

            connection.Open();
            cmd = new MySqlCommand("select * from employeedatabase.employeedata where not (" + days[gridRow] + "_Start <=" + time + " and " + days[gridRow] + "_End >" + time + ");", connection);
            displayCount = 0;
            empReader = cmd.ExecuteReader();
            if (empReader.HasRows)
            {
                suggestionStack.Children.Add(new suggestion("Others", 0, 3, 0, 0, 0));
            }
            while (empReader.Read())
            {
                int displayRow = (displayCount) / (days.Count());
                int displayColumn = displayCount % days.Count() + 1;
                suggestionStack.Children.Add(new suggestion(empReader["First_Name"].ToString(), Convert.ToInt32(empReader["Id"]), displayRow+3, displayColumn, gridRow, gridColumn));
                displayCount++;
            }
            connection.Close();
        }
        private void fetchBtn_Click(object sender, RoutedEventArgs e)
        {
            fetch();
        }

        public void fetch()
        {
            //SUNDAY

            string[] shifts = { "shift1", "shift2", "shift3", "shift4", "shift5", "shift6", "shift7", "shift8", "shift9", "shift10", "shift11", "shift12", "shift13", "shift14", "shift15", "shift16", "shift17", "shift18", "shift19", "shift20", "shift21", "shift22", "shift23", "shift24", };
            List<int> ReturnedSchedule = new List<int>();
            int i;

            string connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            MySqlConnection connection = new MySqlConnection(connectionstring);
            MySqlCommand cmd = new MySqlCommand("select * from employeedatabase.employeeshift where Day='Sunday'", connection);
            connection.Open();
            MySqlDataReader empReader = cmd.ExecuteReader();

            while (empReader.Read())
            {
                for (i = 0; i < 24; ++i)
                {
                    if (DBNull.Value.Equals(empReader[shifts[i]]))
                        break;
                    else
                        ReturnedSchedule.Add(Convert.ToInt32(empReader[shifts[i]]));
                }
                Display(ReturnedSchedule.ToArray(), 0);
                
                if (DateTime.Now.DayOfWeek.ToString() == "Sunday")
                {
                    FrontPageDisplay(ReturnedSchedule.ToArray());
                }
                ReturnedSchedule.Clear();
            }

            //close the mysql connection
            connection.Close();


            //MONDAY

            connection = new MySqlConnection(connectionstring);
            cmd = new MySqlCommand("select * from employeedatabase.employeeshift where day='Monday'", connection);
            connection.Open();
            empReader = cmd.ExecuteReader();

            while (empReader.Read())
            {
                for (i = 0; i < 24; ++i)
                {
                    if (DBNull.Value.Equals(empReader[shifts[i]]))
                        break;
                    else
                        ReturnedSchedule.Add(Convert.ToInt32(empReader[shifts[i]]));
                }
                Display(ReturnedSchedule.ToArray(), 1);
                if (DateTime.Now.DayOfWeek.ToString() == "Monday")
                {
                    FrontPageDisplay(ReturnedSchedule.ToArray());
                }
                ReturnedSchedule.Clear();

            }

            //close the mysql connection
            connection.Close();


            //TUESDAY

            connection = new MySqlConnection(connectionstring);
            cmd = new MySqlCommand("select * from employeedatabase.employeeshift where day='Tuesday'", connection);
            connection.Open();
            empReader = cmd.ExecuteReader();

            while (empReader.Read())
            {
                for (i = 0; i < 24; ++i)
                {
                    if (DBNull.Value.Equals(empReader[shifts[i]]))
                        break;
                    else
                        ReturnedSchedule.Add(Convert.ToInt32(empReader[shifts[i]]));
                }
                Display(ReturnedSchedule.ToArray(), 2);
                if (DateTime.Now.DayOfWeek.ToString() == "Tuesday")
                {
                    FrontPageDisplay(ReturnedSchedule.ToArray());
                }
                ReturnedSchedule.Clear();

            }

            //close the mysql connection
            connection.Close();



            //WEDNESDAY

            connection = new MySqlConnection(connectionstring);
            cmd = new MySqlCommand("select * from employeedatabase.employeeshift where day='Wednesday'", connection);
            connection.Open();
            empReader = cmd.ExecuteReader();

            while (empReader.Read())
            {
                for (i = 0; i < 24; ++i)
                {
                    if (DBNull.Value.Equals(empReader[shifts[i]]))
                        break;
                    else
                        ReturnedSchedule.Add(Convert.ToInt32(empReader[shifts[i]]));
                }
                Display(ReturnedSchedule.ToArray(), 3);
                if (DateTime.Now.DayOfWeek.ToString() == "Wednesday")
                {
                    FrontPageDisplay(ReturnedSchedule.ToArray());
                }
                ReturnedSchedule.Clear();

            }

            //close the mysql connection
            connection.Close();


            //thursday

            connection = new MySqlConnection(connectionstring);
            cmd = new MySqlCommand("select * from employeedatabase.employeeshift where day='thursday'", connection);
            connection.Open();
            empReader = cmd.ExecuteReader();

            while (empReader.Read())
            {
                for (i = 0; i < 24; ++i)
                {
                    if (DBNull.Value.Equals(empReader[shifts[i]]))
                        break;
                    else
                        ReturnedSchedule.Add(Convert.ToInt32(empReader[shifts[i]]));
                }
                Display(ReturnedSchedule.ToArray(), 4);
                if (DateTime.Now.DayOfWeek.ToString() == "Thursday")
                {
                    FrontPageDisplay(ReturnedSchedule.ToArray());
                }
                ReturnedSchedule.Clear();

            }

            //close the mysql connection
            connection.Close();


            //FRIDAY

            connection = new MySqlConnection(connectionstring);
            cmd = new MySqlCommand("select * from employeedatabase.employeeshift where day='Friday'", connection);
            connection.Open();
            empReader = cmd.ExecuteReader();

            while (empReader.Read())
            {
                for (i = 0; i < 24; ++i)
                {
                    if (DBNull.Value.Equals(empReader[shifts[i]]))
                        break;
                    else
                        ReturnedSchedule.Add(Convert.ToInt32(empReader[shifts[i]]));
                }
                Display(ReturnedSchedule.ToArray(), 5);
                if (DateTime.Now.DayOfWeek.ToString() == "Friday")
                {
                    FrontPageDisplay(ReturnedSchedule.ToArray());
                }
                ReturnedSchedule.Clear();

            }

            //close the mysql connection
            connection.Close();

        }

        public void fetchFrontPage()
        {
            try
            {
                //SUNDAY

                string[] shifts = { "shift1", "shift2", "shift3", "shift4", "shift5", "shift6", "shift7", "shift8", "shift9", "shift10", "shift11", "shift12", "shift13", "shift14", "shift15", "shift16", "shift17", "shift18", "shift19", "shift20", "shift21", "shift22", "shift23", "shift24", };
                List<int> ReturnedSchedule = new List<int>();
                int i;

                string connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
                MySqlConnection connection = new MySqlConnection(connectionstring);
                MySqlCommand cmd = new MySqlCommand("select * from employeedatabase.employeeshift where Day='Sunday'", connection);
                connection.Open();
                MySqlDataReader empReader = cmd.ExecuteReader();

                while (empReader.Read())
                {
                    for (i = 0; i < 24; ++i)
                    {
                        if (DBNull.Value.Equals(empReader[shifts[i]]))
                            break;
                        else
                            ReturnedSchedule.Add(Convert.ToInt32(empReader[shifts[i]]));
                    }

                    if (DateTime.Now.DayOfWeek.ToString() == "Sunday")
                    {
                        FrontPageDisplay(ReturnedSchedule.ToArray());
                    }
                    ReturnedSchedule.Clear();
                }

                //close the mysql connection
                connection.Close();


                //MONDAY

                connection = new MySqlConnection(connectionstring);
                cmd = new MySqlCommand("select * from employeedatabase.employeeshift where day='Monday'", connection);
                connection.Open();
                empReader = cmd.ExecuteReader();

                while (empReader.Read())
                {
                    for (i = 0; i < 24; ++i)
                    {
                        if (DBNull.Value.Equals(empReader[shifts[i]]))
                            break;
                        else
                            ReturnedSchedule.Add(Convert.ToInt32(empReader[shifts[i]]));
                    }
                    if (DateTime.Now.DayOfWeek.ToString() == "Monday")
                    {
                        FrontPageDisplay(ReturnedSchedule.ToArray());
                    }
                    ReturnedSchedule.Clear();

                }

                //close the mysql connection
                connection.Close();


                //TUESDAY

                connection = new MySqlConnection(connectionstring);
                cmd = new MySqlCommand("select * from employeedatabase.employeeshift where day='Tuesday'", connection);
                connection.Open();
                empReader = cmd.ExecuteReader();

                while (empReader.Read())
                {
                    for (i = 0; i < 24; ++i)
                    {
                        if (DBNull.Value.Equals(empReader[shifts[i]]))
                            break;
                        else
                            ReturnedSchedule.Add(Convert.ToInt32(empReader[shifts[i]]));
                    }
                    if (DateTime.Now.DayOfWeek.ToString() == "Tuesday")
                    {
                        FrontPageDisplay(ReturnedSchedule.ToArray());
                    }
                    ReturnedSchedule.Clear();

                }

                //close the mysql connection
                connection.Close();



                //WEDNESDAY

                connection = new MySqlConnection(connectionstring);
                cmd = new MySqlCommand("select * from employeedatabase.employeeshift where day='Wednesday'", connection);
                connection.Open();
                empReader = cmd.ExecuteReader();

                while (empReader.Read())
                {
                    for (i = 0; i < 24; ++i)
                    {
                        if (DBNull.Value.Equals(empReader[shifts[i]]))
                            break;
                        else
                            ReturnedSchedule.Add(Convert.ToInt32(empReader[shifts[i]]));
                    }
                    if (DateTime.Now.DayOfWeek.ToString() == "Wednesday")
                    {
                        FrontPageDisplay(ReturnedSchedule.ToArray());
                    }
                    ReturnedSchedule.Clear();

                }

                //close the mysql connection
                connection.Close();


                //thursday

                connection = new MySqlConnection(connectionstring);
                cmd = new MySqlCommand("select * from employeedatabase.employeeshift where day='thursday'", connection);
                connection.Open();
                empReader = cmd.ExecuteReader();

                while (empReader.Read())
                {
                    for (i = 0; i < 24; ++i)
                    {
                        if (DBNull.Value.Equals(empReader[shifts[i]]))
                            break;
                        else
                            ReturnedSchedule.Add(Convert.ToInt32(empReader[shifts[i]]));
                    }
                    if (DateTime.Now.DayOfWeek.ToString() == "Thursday")
                    {
                        FrontPageDisplay(ReturnedSchedule.ToArray());
                    }
                    ReturnedSchedule.Clear();

                }

                //close the mysql connection
                connection.Close();


                //FRIDAY

                connection = new MySqlConnection(connectionstring);
                cmd = new MySqlCommand("select * from employeedatabase.employeeshift where day='Friday'", connection);
                connection.Open();
                empReader = cmd.ExecuteReader();

                while (empReader.Read())
                {
                    for (i = 0; i < 24; ++i)
                    {
                        if (DBNull.Value.Equals(empReader[shifts[i]]))
                            break;
                        else
                            ReturnedSchedule.Add(Convert.ToInt32(empReader[shifts[i]]));
                    }
                    if (DateTime.Now.DayOfWeek.ToString() == "Friday")
                    {
                        FrontPageDisplay(ReturnedSchedule.ToArray());
                    }
                    ReturnedSchedule.Clear();

                }

                //close the mysql connection
                connection.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("No schedule for today");
            }

        }

        private void photoAddBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Image files | *.jpg";
                if (openFileDialog1.ShowDialog() == true)
                {
                    path.Text = openFileDialog1.FileName;
                    empImage.Source = new BitmapImage(new Uri(openFileDialog1.FileName));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void load1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Image files | *.jpg";
                if (openFileDialog1.ShowDialog() == true)
                {
                    path1.Text = openFileDialog1.FileName;
                    updatedImage.Source = new BitmapImage(new Uri(openFileDialog1.FileName));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public class TimeSchedule
    {
        public string Name { set; get; }
        public string Time { set; get; }
        public int Id { set; get; }
        public TimeSchedule(string _name, int _hour, int _id)
        {
            Name = _name;
            Time = Convert.ToString(_hour + Globals.STARTING_HOUR);
            Id = _id;
        }

    }

}
