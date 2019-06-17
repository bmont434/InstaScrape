using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;

namespace InstaScrape
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (txtPassword.Text == "" || txtUsername.Text == "")
            {
                MessageBox.Show("Please enter username and password");
            }
            else
            {
                ScrapeInsta();
            }
            
        }

        public void ScrapeInsta()
        {
            FirefoxOptions option = new FirefoxOptions();
            option.AddArguments("--headless");
            IWebDriver driver = new FirefoxDriver(option);
            string instuser = txtUsername.Text; ;
            

            driver.Url = "https://www.instagram.com/" +txtUsername.Text+"";
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            var followerbuttn = driver.FindElement(By.CssSelector("li.Y8-fY:nth-child(2) > a:nth-child(1)"));
            

            try
            {
                
                followerbuttn.Click();
                var username = driver.FindElement(By.Name("username"));
                username.SendKeys(instuser);
                username.SendKeys(OpenQA.Selenium.Keys.Tab + txtPassword.Text);
            }
            catch
            {
                var NotNow = driver.FindElement(By.CssSelector(".g6RW6"));
                NotNow.Click();
                followerbuttn.Click();
                var username = driver.FindElement(By.Name("username"));
                username.SendKeys(instuser);
                username.SendKeys(OpenQA.Selenium.Keys.Tab + txtPassword.Text);
            }
     
            var login = driver.FindElement(By.CssSelector("div.Igw0E:nth-child(4)"));
            login.Click();
            TimeSpan.FromSeconds(1);
            followerbuttn = driver.FindElement(By.CssSelector("li.Y8-fY:nth-child(2) > a:nth-child(1)"));
            followerbuttn.Click();

            List<string>flwrs = new List<string>();
            List<string> flwing = new List<string>();
            List<string> Nonfollowers = new List<string>();


            for (int i = 1;i < 10000000; ++i)
            {
                try
                {
                    int ad = i + 1;
                    var bit = driver.FindElement(By.CssSelector("li.wo9IH:nth-child(" + i + ") > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > a:nth-child(1)"));

                    string fls = bit.GetAttribute("innerHTML");
                    flwrs.Add(fls);
                    //textBox1.Text += fls;
                    //textBox1.AppendText(Environment.NewLine);

                    IJavaScriptExecutor js = driver as IJavaScriptExecutor;

                    var elem = driver.FindElement(By.CssSelector("li.wo9IH:nth-child("+ad+") > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > a:nth-child(1)"));
                    js.ExecuteScript("arguments[0].scrollIntoView(true);", elem);
                }
                catch
                {
                    break;
                }    
            }

            var x = driver.FindElement(By.CssSelector(".glyphsSpriteX__outline__24__grey_9"));
            x.Click();

            var following = driver.FindElement(By.CssSelector("li.Y8-fY:nth-child(3) > a:nth-child(1)"));
            following.Click();
            textBox1.Clear();

            for (int i = 1; i < 10000000; ++i)
            {
                try
                {
                    int ad = i + 1;
                    var bit = driver.FindElement(By.CssSelector("li.wo9IH:nth-child("+ i +") > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > a:nth-child(1)"));

                    string fling = bit.GetAttribute("innerHTML");
                    flwing.Add(fling);
                    //textBox1.Text += fling;
                    //textBox1.AppendText(Environment.NewLine);

                    IJavaScriptExecutor js = driver as IJavaScriptExecutor;

                    var elem = driver.FindElement(By.CssSelector("li.wo9IH:nth-child(" + ad + ") > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > a:nth-child(1)"));
                    js.ExecuteScript("arguments[0].scrollIntoView(true);", elem);
                }
                catch
                {
                    break;
                }
            }

            
            int countr = 0;
            for (int i = 0; i < flwing.Count(); ++i )
            {
                countr = 0;
                for (int j = 0; j < flwrs.Count; ++j)
                {
                    if (flwing[i] == flwrs[j])
                    {
                        countr = 1;
                    }
                }
            if (countr != 1)
            {
                    Nonfollowers.Add(flwing[i]);
            }
            }
            textBox1.Clear();

            for (int k = 0; k < Nonfollowers.Count(); ++k)
            {
                textBox1.Text += Nonfollowers[k];
                textBox1.AppendText(Environment.NewLine);
            }

        }
    }
}
