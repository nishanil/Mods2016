using System;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;

namespace MyEvents.UITests
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
    public class Tests
    {
		IApp app;
        const string UserName = "";
        const string Password = "";
		readonly Platform platform;

		public Tests(Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
        public void BeforeEachTest()
        {
			// TODO: If the Android app being tested is included in the solution then open
			// the Unit Tests window, right click Test Apps, select Add App Project
			// and select the app projects that should be tested.

			if (platform == Platform.Android)
			{
				app = ConfigureApp
					.Android
						.StartApp();
				return;
			}

			app = ConfigureApp.iOS.StartApp();
        }

        [Test]
        public void WhenAppStartsLoadTheSessions()
        {
            app.WaitForElement(a => a.Marked("SessionsList"), timeout: TimeSpan.FromSeconds(45));
			app.ScrollDown(a => a.Marked("SessionsList"));
        }

        [Test]
        public void GivenASessionsListWhenITapOnTheSessionThenShowDetails()
        {
            app.WaitForElement("SessionCell");
            app.Tap("SessionCell");
            app.WaitForElement("Abstract");
        }

        [Test]
        public void GivenSessionsDetailPageWhenTappedOnBackThenSessionsListShowShouldShow()
        {
            app.WaitForElement("SessionCell");
            app.Tap("SessionCell");

            app.Screenshot("Session Details");
            app.Back();

            app.WaitForElement("SessionCell");
           
        }

        [Test]
        public void GivenASessionWhenWhenTappedOnFeedbackThenAskToLogin()
        {
            LoginToFeedback();

            app.WaitForElement("Authenticate");
            app.Screenshot("Facebook login page");
        }

        [Test]
        public void WhenPresentedLoginPageThenLoginWithFacebook()
        {
            LoginToFeedback();

			if (platform == Platform.iOS)
			{
				app.Tap(x => x.Class("UIWebView").Css("INPUT._56bg._4u9z._5ruq"));
				app.Screenshot("Tapped on view with class: UIWebView");
				app.EnterText(x => x.Class("UIWebView").Css("INPUT._56bg._4u9z._5ruq"), UserName);
				app.Tap(x => x.Class("UIWebView").Css("INPUT#u_0_2"));
				app.Screenshot("Tapped on view with class: UIWebView");
				app.EnterText(x => x.Class("UIWebView").Css("INPUT#u_0_2"), Password);
				app.Tap(x => x.Class("UIWebView").Css("BUTTON#u_0_6"));
				//app.Back();

			}
			else
			{
				app.EnterText(c => c.WebView().Css("input").Index(0), UserName);
				app.Back();
				app.EnterText(c => c.WebView().Css("input").Index(1), Password);


				app.Back();
				app.Tap(c => c.WebView().Css("Button"));

				var continueButtone = app.Query(c => c.Css("Button#u_0_a"));

				if (continueButtone.Length > 0)
				{
					app.Query(c => c.Css("Button#u_0_a"));
					app.Screenshot("Continue with authentication");
				}
			}
            app.WaitForElement("FEEDBACK", timeout:TimeSpan.FromSeconds(45));
        }

        [Test]
        public void WhenOnFeedbackPageLeaveFeedback()
        {
            LoginToFeedback();

			//app.Repl();

			if (platform == Platform.iOS)
			{
				app.Tap(x => x.Class("UIWebView").Css("INPUT._56bg._4u9z._5ruq"));
				app.Screenshot("Tapped on view with class: UIWebView");
				app.EnterText(x => x.Class("UIWebView").Css("INPUT._56bg._4u9z._5ruq"), UserName);
				app.Tap(x => x.Class("UIWebView").Css("INPUT#u_0_2"));
				app.Screenshot("Tapped on view with class: UIWebView");
				app.EnterText(x => x.Class("UIWebView").Css("INPUT#u_0_2"), Password);
				app.Tap(x => x.Class("UIWebView").Css("BUTTON#u_0_6"));
				//app.Back();

			}
			else
			{
				app.EnterText(c => c.WebView().Css("input").Index(0), UserName);
				app.Back();
				app.EnterText(c => c.WebView().Css("input").Index(1), Password);
				app.Back();
				app.Tap(c => c.WebView().Css("Button"));

				var continueButtone = app.Query(c => c.Css("Button#u_0_a"));

				if (continueButtone.Length > 0)
				{
					app.Query(c => c.Css("Button#u_0_a"));
				}
			}

            app.WaitForElement("FEEDBACK");
            
            app.Tap(c => c.Class("FormsImageView").Index(4));
            app.EnterText("FeedbackText", $"Meh! on {DateTime.Now}");

            app.Screenshot("Entered the feedback");

            app.Back();
            app.Tap("Send");

            app.WaitForElement(a => a.Button("Feedback"));
        }

        [Test]
        public void LoadSpeakersList()
        {  
            app.Tap("Speakers");
			app.ScrollDown("SpeakerList");
        }

        [Test]
        public void GivenASpeakersOnSelectingOneDiscloseTheDetails()
        {
            app.Tap("Speakers");
            app.Tap(c => c.Class("TextView"));
            app.WaitForElement("SpeakerImage");
        }

        [Test]
        public void WhenSelectedAboutTabThenLoadAboutPage()
        {
            app.Tap("About");
            app.WaitForElement("Learn More");
        }

        void LoginToFeedback()
        {
            app.WaitForElement("SessionCell");
            app.Tap("SessionCell");
			app.ScrollDown(a => a.Marked("SessionDetails"));
            //app.ScrollDownTo(c => c.Marked("Feedback"));
            app.Tap("Feedback");
            app.Tap("Login");
            app.Screenshot("Login page");
        }


		//[Test]
		//public void NewTest()
		//{
		//	app.Tap(x => x.Class("UIImageView").Index(18));
		//	app.Screenshot("Tapped on view with class: UIImageView");
		//	app.ScrollDownTo("Give Feedback", withinMarked: "SessionDetails");
		//	app.Screenshot("ScrollToEvent[AppView: Class=Xamarin.TestRecorder.Portable.Models.Class, Id=, Text=Give Feedback, Marked=Give Feedback, Css=, XPath=, IndexInTree=-1, Rect=[Rectangle: Left=137, Top=547, CenterX=187.75, CenterY=556, Width=101.5, Height=18, Bottom=565, Right=238.5]]");
		//	app.Tap(x => x.Text("Give Feedback"));
		//	app.Screenshot("Tapped on view with class: UIButtonLabel marked: Give Feedback");
		//	app.Tap(x => x.Id("Login"));
		//	app.Screenshot("Tapped on view with class: UIButton marked: LOGIN WITH FACEBOOK");

		//}

	}
}

