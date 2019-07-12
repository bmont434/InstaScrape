/*
 *Author: Beau H.
 * edited 7/12/19
 */

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
    public partial class ScrapeForm : Form
    {
        public ScrapeForm()
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
            //creating headless firefox webdriver
            FirefoxOptions option = new FirefoxOptions();
           option.AddArguments("--headless");
            IWebDriver driver = new FirefoxDriver(option);

            //setting username equal to textbox
            string instuser = txtUsername.Text; ;
            
            //Ensuring URL goes to desired Insta profile
            driver.Url = "https://www.instagram.com/" +txtUsername.Text+"";
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            //locating the follower button
            var followerbuttn = driver.FindElement(By.CssSelector("li.Y8-fY:nth-child(2) > a:nth-child(1)"));
            
            //try catch must be used to sign in due to Instagram's dynamic code
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
            
            //logging in and locating the follower button
            var login = driver.FindElement(By.CssSelector("div.Igw0E:nth-child(4)"));
            login.Click();
            TimeSpan.FromSeconds(1);
            followerbuttn = driver.FindElement(By.CssSelector("li.Y8-fY:nth-child(2) > a:nth-child(1)"));
            followerbuttn.Click();

            //creating lists to store followers, following, and the non followers
            List<string>flwrs = new List<string>();
            List<string> flwing = new List<string>();
            List<string> Nonfollowers = new List<string>();


            for (int i = 1;; ++i)
            {
                try
                {
                    //adding followers to a list and scrolling the page with JS
                    int scroll = i + 1;
                    var FlwrsName = driver.FindElement(By.CssSelector("li.wo9IH:nth-child(" + i + ") > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > a:nth-child(1)"));

                    string fls = FlwrsName.GetAttribute("innerHTML");
                    flwrs.Add(fls);

                    IJavaScriptExecutor js = driver as IJavaScriptExecutor;

                    var elem = driver.FindElement(By.CssSelector("li.wo9IH:nth-child("+scroll+") > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > a:nth-child(1)"));
                    js.ExecuteScript("arguments[0].scrollIntoView(true);", elem);
                }
                catch
                {
                    break;
                }    
            }

            //this "x" represents the x clicked to exit followers page
            var x = driver.FindElement(By.CssSelector(".glyphsSpriteX__outline__24__grey_9"));
            x.Click();

            var following = driver.FindElement(By.CssSelector("li.Y8-fY:nth-child(3) > a:nth-child(1)"));
            following.Click();
            textBox1.Clear();

            for (int i = 1;; ++i)
            {
                try
                {
                    //adding scraped following to a list and scrolling with JS
                    int scroll = i + 1;
                    var bit = driver.FindElement(By.CssSelector("li.wo9IH:nth-child("+ i +") > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > a:nth-child(1)"));

                    string fling = bit.GetAttribute("innerHTML");
                    flwing.Add(fling);
                   
                    IJavaScriptExecutor js = driver as IJavaScriptExecutor;

                    var elem = driver.FindElement(By.CssSelector("li.wo9IH:nth-child(" + scroll + ") > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > a:nth-child(1)"));
                    js.ExecuteScript("arguments[0].scrollIntoView(true);", elem);
                }
                catch
                {
                    break;
                }
            }

            driver.Close();
            driver.Quit();

            //generating non followers by comparing following and followers lists
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
