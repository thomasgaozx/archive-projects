using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebAutomation.YoutubeAdSkipper.Forms
{
    public static class Prompts
    {

        #region Nested Type

        public class User
        {
            public string UserName { get; set; }
            public string PassWord { get; set; }

            public User() { }
            public User(string userName, string pswd)
            {
                UserName = userName;
                PassWord = pswd;
            }
        }

        #endregion

        #region Protected Field

        public static readonly Regex EmailPattern = new Regex(@".+@.+\..+");

        #endregion
        
        #region Forms



        public static User PromptPswdProcessor()
        {
            string name = "";
            string pswd = "";

            const int TotalWidth = 300;
            const int DefaultWidth = 200;
            int LeftMargin = (TotalWidth - DefaultWidth) / 2;
            Form prompt = new Form()
            {
                AutoSize = true,
                Text = "Enter User Information",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label userName = new Label() { Left = LeftMargin, Top = 20, Text = "Enter User Email: " };
            TextBox nameBox = new TextBox() { Left = LeftMargin, Top = 50, Width = DefaultWidth };
            Label pswdLabel = new Label() { Left = LeftMargin, Top = 80, Text = "Enter Password: " };
            TextBox pswdBox = new TextBox() { Left = LeftMargin, Top = 110, Width = DefaultWidth, UseSystemPasswordChar = true };
            Button submitButton = new Button()
            {
                Text = "Submit",
                Left = (TotalWidth - 100) / 2,
                Width = 100,
                Top = 150,
                DialogResult = DialogResult.OK
            };

            submitButton.Click += (sender, e) =>
            {
                name = nameBox.Text;
                pswd = pswdBox.Text;
                prompt.Close();
            };

            prompt.Controls.Add(userName);
            prompt.Controls.Add(nameBox);
            prompt.Controls.Add(pswdLabel);
            prompt.Controls.Add(pswdBox);
            prompt.Controls.Add(submitButton);
            prompt.AcceptButton = submitButton;

            if (!(prompt.ShowDialog() == DialogResult.OK))
            {
                throw new Exception("Bad form request");
            }

            if (string.IsNullOrWhiteSpace(nameBox.Text))
            {
                Console.Write("Name box is empty. ");
            }
            else if (EmailPattern.Match(nameBox.Text).Value.Equals(""))
            {
                Console.Write("Invalid email. ");
            }
            else if (string.IsNullOrWhiteSpace(pswdBox.Text))
            {
                Console.Write("Password cannot be empty. ");
            }
            else
            {
                return new User(name, pswd);
            }

            return null;
        }

        public static string ReEnterInformation(bool isPswd = false)
        {
            string field = "";

            const int TotalWidth = 300;
            const int DefaultWidth = 200;
            int LeftMargin = (TotalWidth - DefaultWidth) / 2;
            Form prompt = new Form()
            {
                Width = TotalWidth,
                AutoSize = true,
                Text = "ReEnter User Information",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };

            string labelName = isPswd ? "Password" : "user information";
            Label infoLabel = new Label() { Left = LeftMargin, Top = 20, Text = $"Enter {labelName}: " };
            TextBox infoBox = new TextBox() { Left = LeftMargin, Top = 50, Width = DefaultWidth, UseSystemPasswordChar = isPswd };
            Button submitButton = new Button()
            {
                Text = "Submit",
                Left = (TotalWidth - 100) / 2,
                Width = 100,
                Top = 80,
                DialogResult = DialogResult.OK
            };

            submitButton.Click += (sender, e) =>
            {
                field = infoBox.Text;
                prompt.Close();
            };

            prompt.Controls.Add(infoLabel);
            prompt.Controls.Add(infoBox);
            prompt.Controls.Add(submitButton);
            prompt.AcceptButton = submitButton;

            if (!(prompt.ShowDialog() == DialogResult.OK))
            {
                throw new Exception("Bad form request");
            }

            if (string.IsNullOrWhiteSpace(infoBox.Text))
            {
                Console.Write("Field is empty! ");
            }
            else
            {
                return field;
            }

            return null;
        }

        #endregion

    }
}
