using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Wallet;
using System.Windows.Media.Imaging;
using MySilverlightWallet.Resources;

namespace MySilverlightWallet
{
    public static class MyWalletExtension
    {
        public static void AssignCivicAddress(this WalletAddress walletAddress, System.Device.Location.CivicAddress civicAddress)
        {
            walletAddress.City = civicAddress.City;
            walletAddress.CountryRegion = civicAddress.CountryRegion;
            walletAddress.PostalCode = civicAddress.PostalCode;
            walletAddress.StateProvince = civicAddress.StateProvince;
            walletAddress.Street = civicAddress.AddressLine1;
        }
    }

    public partial class MainPage : PhoneApplicationPage
    {

        private string membershipCardQRName = "Caffe Luzzo";
        private string membershipCardBarcodeName = "Bella Vita Cafe";
        private string dealName = "Contoso Deal";
        private string airlineName = "Antarctica Airlines";
        private string concertName = "WLR Concert";
        private string expiredDealName = "Expired Contoso Deal";
        private string paymentInstrumentName = "Litecoin";
        private string generalItemName = "Contoso Card";

        AddWalletItemTask task;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            this.task = new AddWalletItemTask();
            task.Completed += task_Completed;

            ClearAllItems();
        }

        private void ClearAllItems()
        {
            Wallet.Clear();
        }


        void task_Completed(object sender, AddWalletItemResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                //MessageBox.Show(e.Item.Id + " was added to your Wallet");
            } 
    
        }
        
        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png",UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        #region Membership
        private void AddMembershipButtonQR_Click(object sender, RoutedEventArgs e)
        {
            //string itemId = string.IsNullOrEmpty(walletItemName.Text) ? walletItemName.Text : cardName;

            WalletTransactionItem card = new WalletTransactionItem();
            card.Nickname = membershipCardQRName;
            // Set colors, to give the card our distinct branding.
            card.BackgroundColor = System.Windows.Media.Colors.Brown;

            var bmp = new BitmapImage(new Uri("/Assets/icons/CaffeLuzzoMedium.png", UriKind.RelativeOrAbsolute));
            bmp.CreateOptions = BitmapCreateOptions.None;
            card.Logo336x336 = bmp;

            bmp = new BitmapImage(new Uri("/Assets/icons/CaffeLuzzoSmall.png", UriKind.RelativeOrAbsolute));
            bmp.CreateOptions = BitmapCreateOptions.None;
            card.Logo159x159 = bmp;

            bmp = new BitmapImage(new Uri("/Assets/icons/CaffeLuzzoIcon.png", UriKind.RelativeOrAbsolute));
            bmp.CreateOptions = BitmapCreateOptions.None;
            card.Logo99x99 = bmp;

            //card.UserImage = new BitmapImage(new Uri("/Assets/icons/CaffeLuzzoIcon.png", UriKind.Relative));
            card.DisplayName = membershipCardQRName;

            // Add the customer account number.
            CustomWalletProperty  prop = new CustomWalletProperty ("HOLD", "HOLD");
            card.CustomProperties["Holder1"] = prop;

            prop = new CustomWalletProperty ("HOLD", "HOLD");
            card.CustomProperties["Holder2"] = prop;

            // Add the customer account number.
            prop = new CustomWalletProperty ("Caffe Luzzo", "Membership Card");
            card.CustomProperties["Title"] = prop;
            card.IssuerName = "Caffe Luzzo";

            prop = new CustomWalletProperty("Website", "http://www.caffelusso.com/#!");
            card.CustomProperties["Website"] = prop;
            card.IssuerWebsite = new Uri("http://www.caffelusso.us");

            prop = new CustomWalletProperty ("Address", "17725 Ne 65th St Ste A150, Redmond, WA 98052 ");
            card.CustomProperties["Address"] = prop;
            card.IssuerAddress.Business1.AssignCivicAddress(new CivicAddress()
            {
                AddressLine1 = "17725 Ne 65th St Ste A150",
                City = "Redmond",
                StateProvince = "WA",
                PostalCode = "98052"
            });

            prop = new CustomWalletProperty ("Rewards", "99");
            card.CustomProperties["Points"] = prop;
            card.DisplayAvailableBalance = "100";

            // Add the customer account number.
            prop = new CustomWalletProperty ("Account Number", "12345678");
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.CustomProperties["AcctId"] = prop;
            card.AccountNumber = "12345678";

            // Encode the user's account number as a Qr Code to be used in the store.
            //card.Barcode = new WalletBarcode(WalletBarcodeSymbology.Qr, "12345678");

            // Add a promotional message to the card.
            card.Message = "Tap here for your 15% off coupon";

            ((Button)sender).IsEnabled = false;
            this.MembershipInfoButtonQR.IsEnabled = true;

            task.Item = card;
            task.Show();
        }

        private void ClearMembershipButton_Click(object sender, RoutedEventArgs e)
        {
            this.AddMembershipButtonQR.IsEnabled = true;
            this.MembershipInfoButtonQR.IsEnabled = false;
            this.AddMembershipButtonBarCode.IsEnabled = true;
            this.MembershipInfoButtonBarcode.IsEnabled = false;

            if (Wallet.FindItem(membershipCardQRName)!=null)
            {
                Wallet.Remove(membershipCardQRName);
            }
            if (Wallet.FindItem(membershipCardBarcodeName)!=null)
            {
                Wallet.Remove(membershipCardBarcodeName);
            }
        }

        private  void MembershipQRInfo_Click(object sender, RoutedEventArgs e)
        {
            var item = Wallet.FindItem(membershipCardQRName);
        }


        private  void AddMembershipButtonBarcode_Click(object sender, RoutedEventArgs e)
        {

            WalletTransactionItem card = new WalletTransactionItem();
            card.Nickname = membershipCardBarcodeName;
            // Set colors, to give the card our distinct branding.

            card.BackgroundColor = System.Windows.Media.Colors.Brown;


            card.Logo336x336 = new BitmapImage(new Uri("/Assets/icons/bellavita_cafe_Medium.png", UriKind.RelativeOrAbsolute));
            card.Logo159x159 = new BitmapImage(new Uri("/Assets/icons/bellavita_cafe_Small.png", UriKind.RelativeOrAbsolute));
            card.Logo99x99 = new BitmapImage(new Uri("/Assets/icons/bellavita_cafe_Icon.png", UriKind.RelativeOrAbsolute));

            //card.UserImage = new BitmapImage(new Uri("Assets/icons/bellavita_cafe_Icon.png", UriKind.Relative));
            card.DisplayName= membershipCardBarcodeName;

            // Add the customer account number.
            CustomWalletProperty  prop = new CustomWalletProperty ("HOLD", "HOLD");
            card.CustomProperties["Holder1"] = prop;

            prop = new CustomWalletProperty ("HOLD", "HOLD");
            card.CustomProperties["Holder2"] = prop;

            // Add the customer account number.
            prop = new CustomWalletProperty ("Cafe Bella", "Membership Card");
            //prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.CustomProperties["Title"] = prop;
            card.IssuerName = "Cafe Bella";

            prop = new CustomWalletProperty ("Website", "http://www.lavitaebella.us/");
            //prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField2;
            card.CustomProperties["Website"] = prop;
            card.IssuerWebsite = new Uri("http://www.lavitaebella.us");

            prop = new CustomWalletProperty ("Address", "2411 2nd Ave, Seattle, WA 98121");
            card.CustomProperties["Address"] = prop;
            card.IssuerAddress.Business1.AssignCivicAddress(new CivicAddress()
            {
                AddressLine1 = "2411 2nd Ave",
                City = "Seattle",
                StateProvince = "WA",
                PostalCode = "98121"
            });

            prop = new CustomWalletProperty ("Rewards", "100");
            card.CustomProperties["Points"] = prop;
            card.DisplayAvailableBalance = "100";

            // Add the customer account number.
            prop = new CustomWalletProperty ("Account Number", "987654321");
            card.CustomProperties["AcctId"] = prop;
            card.AccountNumber = "987654321";

            // Encode the user's account number as a Qr Code to be used in the store.
            //card.BarcodeImage = new Barcode 
            //    new WalletBarcode(WalletBarcodeSymbology.Ean13, "9876543210123");

            // Add a promotional message to the card.
            card.Message = "Tap here for your 15% off coupon";
            //card.IsDisplayMessageLaunchable = true;

            ((Button)sender).IsEnabled = false;
            this.MembershipInfoButtonBarcode.IsEnabled = true;

            task.Item = card;
            task.Show();
        }

        private void MembershipInfoButtonBarcode_Click(object sender, RoutedEventArgs e)
        {

            var item = Wallet.FindItem(membershipCardBarcodeName);
            
        }


        #endregion
    }
}