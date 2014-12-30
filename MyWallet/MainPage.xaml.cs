using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Wallet;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace MyWallet
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string membershipCardQRName = "Caffe Luzzo";
        private string membershipCardBarcodeName = "Bella Vita Cafe";
        private string dealName = "Contoso Deal";
        private string airlineName = "Antarctica Airlines";
        private string concertName = "WLR Concert";
        private string expiredDealName = "Expired Contoso Deal";
        private string paymentInstrumentName = "Litecoin";
        private string generalItemName = "Contoso Card";

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            ClearAllItems();
        }

        private async void ClearAllItems()
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();
            await store.ClearAsync();
        }
        
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        #region Membership
        private async void AddMembershipButtonQR_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();

            //string itemId = string.IsNullOrEmpty(walletItemName.Text) ? walletItemName.Text : cardName;

            WalletItem card = new WalletItem(WalletItemKind.MembershipCard, membershipCardQRName);
            // Set colors, to give the card our distinct branding.
            card.BodyColor = Windows.UI.Colors.Brown;
            card.BodyFontColor = Windows.UI.Colors.White;
            card.HeaderColor = Windows.UI.Colors.SaddleBrown;
            card.HeaderFontColor = Windows.UI.Colors.White;


            card.Logo336x336 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/CaffeLuzzoMedium.png"));
            card.Logo159x159 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/CaffeLuzzoSmall.png"));
            card.Logo99x99 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/CaffeLuzzoIcon.png"));

            card.LogoImage = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/CaffeLuzzoIcon.png"));
            card.LogoText = membershipCardQRName;

            // Add the customer account number.
            WalletItemCustomProperty prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.HeaderField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Holder1"] = prop;

            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.HeaderField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Holder2"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("Caffe Luzzo", "Membership Card");
            prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Title"] = prop;

            prop = new WalletItemCustomProperty("Website", "http://www.caffelusso.com/#!");
            prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField2;
            card.DisplayProperties["Website"] = prop;

            prop = new WalletItemCustomProperty("Address", "17725 Ne 65th St Ste A150, Redmond, WA 98052 ");
            prop.DetailViewPosition = WalletDetailViewPosition.SecondaryField1;
            card.DisplayProperties["Address"] = prop;

            prop = new WalletItemCustomProperty("Rewards", "99");
            prop.DetailViewPosition = WalletDetailViewPosition.FooterField1;
            prop.SummaryViewPosition = WalletSummaryViewPosition.Field1;
            card.DisplayProperties["Points"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("Account Number", "12345678");
            prop.DetailViewPosition = WalletDetailViewPosition.FooterField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["AcctId"] = prop;

            // Encode the user's account number as a Qr Code to be used in the store.
            card.Barcode = new WalletBarcode(WalletBarcodeSymbology.Qr, "12345678");

            // Add a promotional message to the card.
            card.DisplayMessage = "Tap here for your 15% off coupon";
            card.IsDisplayMessageLaunchable = true;
            
            card.Verbs.Add("visit", new WalletVerb("Visit Store"));

            ((Button)sender).IsEnabled = false;
            this.MembershipInfoButtonQR.IsEnabled = true;

            await store.AddAsync(membershipCardQRName, card);
        }

        private async void ClearMembershipButton_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();
            this.AddMembershipButtonQR.IsEnabled = true;
            this.MembershipInfoButtonQR.IsEnabled = false;
            this.AddMembershipButtonBarCode.IsEnabled = true;
            this.MembershipInfoButtonBarcode.IsEnabled = false;

            if (await (store.GetWalletItemAsync(membershipCardQRName)) != null)
            {
                await store.DeleteAsync(membershipCardQRName);
            }
            if (await (store.GetWalletItemAsync(membershipCardBarcodeName)) != null)
            {
                await store.DeleteAsync(membershipCardBarcodeName);
            }
        }

        private async void MembershipQRInfo_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();
            await store.ShowAsync(membershipCardQRName);
        }


        private async void AddMembershipButtonBarcode_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();

            //string itemId = string.IsNullOrEmpty(walletItemName.Text) ? walletItemName.Text : cardName;

            WalletItem card = new WalletItem(WalletItemKind.MembershipCard, membershipCardBarcodeName);
            // Set colors, to give the card our distinct branding.
            card.BodyColor = Windows.UI.Colors.Brown;
            card.BodyFontColor = Windows.UI.Colors.White;
            card.HeaderColor = Windows.UI.Colors.SaddleBrown;
            card.HeaderFontColor = Windows.UI.Colors.White;


            card.Logo336x336 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/bellavita_cafe_Medium.png"));
            card.Logo159x159 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/bellavita_cafe_Small.png"));
            card.Logo99x99 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/bellavita_cafe_Icon.png"));

            card.LogoImage = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/bellavita_cafe_Icon.png"));
            card.LogoText = membershipCardBarcodeName;

            // Add the customer account number.
            WalletItemCustomProperty prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.HeaderField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Holder1"] = prop;

            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.HeaderField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Holder2"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("Cafe Bella", "Membership Card");
            prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Title"] = prop;

            prop = new WalletItemCustomProperty("Website", "http://www.lavitaebella.us/");
            prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField2;
            card.DisplayProperties["Website"] = prop;


            prop = new WalletItemCustomProperty("Address", "2411 2nd Ave, Seattle, WA 98121");
            prop.DetailViewPosition = WalletDetailViewPosition.SecondaryField1;
            card.DisplayProperties["Address"] = prop;



            prop = new WalletItemCustomProperty("Rewards", "100");
            prop.DetailViewPosition = WalletDetailViewPosition.FooterField1;
            prop.SummaryViewPosition = WalletSummaryViewPosition.Field1;
            card.DisplayProperties["Points"] = prop;


            // Add the customer account number.
            prop = new WalletItemCustomProperty("Account Number", "987654321");
            prop.DetailViewPosition = WalletDetailViewPosition.FooterField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["AcctId"] = prop;

            // Encode the user's account number as a Qr Code to be used in the store.
            card.Barcode = new WalletBarcode(WalletBarcodeSymbology.Ean13, "9876543210123");

            // Add a promotional message to the card.
            card.DisplayMessage = "Tap here for your 15% off coupon";
            card.IsDisplayMessageLaunchable = true;

            ((Button)sender).IsEnabled = false;
            this.MembershipInfoButtonBarcode.IsEnabled = true;

            await store.AddAsync(membershipCardBarcodeName, card);
        }

        private async void MembershipInfoButtonBarcode_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();
            await store.ShowAsync(membershipCardBarcodeName);
        }

        #endregion

        #region Deal
        private async void AddDealPassButtonBarCode_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();

            //string itemId = string.IsNullOrEmpty(walletItemName.Text) ? walletItemName.Text : cardName;

            WalletItem card = new WalletItem(WalletItemKind.Deal, dealName);
            // Set colors, to give the card our distinct branding.
            card.BodyColor = Windows.UI.Colors.Brown;
            card.BodyFontColor = Windows.UI.Colors.White;
            card.HeaderColor = Windows.UI.Colors.SaddleBrown;
            card.HeaderFontColor = Windows.UI.Colors.White;


            card.Logo336x336 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/CouponMedium.png"));
            card.Logo159x159 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/CouponSmall.png"));
            card.Logo99x99 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/CouponIcon.png"));

            card.LogoImage = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/CouponIcon.png"));
            card.LogoText = dealName;

            // Add the customer account number.
            WalletItemCustomProperty prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.HeaderField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Holder1"] = prop;

            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.HeaderField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Holder2"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("Sale", "Christmas Couple");
            prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["SaleTitle"] = prop;

            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Holder3"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("Coupon Code", "987654321");
            prop.DetailViewPosition = WalletDetailViewPosition.SecondaryField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["Coupon Code"] = prop;

            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.SecondaryField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Holder4"] = prop;

            prop = new WalletItemCustomProperty("Valid Until", "2014-12-20");
            //prop.DetailViewPosition = WalletDetailViewPosition.FooterField1;
            prop.AutoDetectLinks = true;
            prop.SummaryViewPosition = WalletSummaryViewPosition.Field1;
            card.DisplayProperties["Website"] = prop;

            prop = new WalletItemCustomProperty("Website", "http://www.Contoso.com/");
            //prop.DetailViewPosition = WalletDetailViewPosition.FooterField2;
            prop.AutoDetectLinks = true;
            prop.SummaryViewPosition = WalletSummaryViewPosition.Field2;
            card.DisplayProperties["Website"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("Phone", "425-707-1234");
            //prop.DetailViewPosition = WalletDetailViewPosition.FooterField3;
            prop.SummaryViewPosition = WalletSummaryViewPosition.Field1;
            prop.AutoDetectLinks = true;
            card.DisplayProperties["phone"] = prop;

            prop = new WalletItemCustomProperty("Location", "1 Microsoft Way, Redmond,  WA 98052");
            //prop.DetailViewPosition = WalletDetailViewPosition.FooterField4;
            prop.AutoDetectLinks = true;
            
            card.DisplayProperties["Address"] = prop;

            // Encode the user's account number as a Qr Code to be used in the store.
            card.Barcode = new WalletBarcode(WalletBarcodeSymbology.Ean13, "9876543210123");

            //card.ExpirationDate = new DateTimeOffset(new DateTime(2015, 1, 1), new TimeSpan(10, 0, 0, 0));

            card.ExpirationDate = new DateTimeOffset(new DateTime(2015, 2, 28));
            //card.ExpirationDate = new DateTimeOffset(DateTime.UtcNow, new TimeSpan(0, 0, 0, 0));

            // NOTE: in the back of the card
            card.LastUpdated = new DateTimeOffset(new DateTime(2014, 12, 10));


            card.IsAcknowledged = true;
            card.IssuerDisplayName = "Contoso Ltd.";
            card.RelevantLocations.Add(
                "Store",
                new WalletRelevantLocation()
                {
                    DisplayMessage = "Store location",
                    Position = new Windows.Devices.Geolocation.BasicGeoposition()
                    {
                        Latitude = 47.640068,
                        Longitude = -122.129858
                    }
                });

            // Add a relevant date.
            card.RelevantDate = DateTime.Now;
            card.RelevantDateDisplayMessage = "Deal is available all the way in 2014!";

            // Add a promotional message to the card.
            card.DisplayMessage = "Tap here for your 15% off coupon";
            card.IsDisplayMessageLaunchable = true;

            ((Button)sender).IsEnabled = false;
            this.DealPassInfoButton.IsEnabled = true;

            await store.AddAsync(dealName, card);
        }

        private async void DealPassInfoButton_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();
            await store.ShowAsync(dealName);
        }


        private async void AddExpiredDealPassButton_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();

            //string itemId = string.IsNullOrEmpty(walletItemName.Text) ? walletItemName.Text : cardName;

            WalletItem card = new WalletItem(WalletItemKind.Deal, dealName);
            // Set colors, to give the card our distinct branding.
            card.BodyColor = Windows.UI.Colors.Brown;
            card.BodyFontColor = Windows.UI.Colors.White;
            card.HeaderColor = Windows.UI.Colors.SaddleBrown;
            card.HeaderFontColor = Windows.UI.Colors.White;


            card.Logo336x336 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/CouponMedium.png"));
            card.Logo159x159 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/CouponSmall.png"));
            card.Logo99x99 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/CouponIcon.png"));

            card.LogoImage = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/CouponIcon.png"));
            card.LogoText = dealName;

            // Add the customer account number.
            WalletItemCustomProperty prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.HeaderField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Holder1"] = prop;

            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.HeaderField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Holder2"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("Sale", "Christmas Couple");
            prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["SaleTitle"] = prop;

            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Holder3"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("Coupon Code", "987654321");
            prop.DetailViewPosition = WalletDetailViewPosition.SecondaryField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["Coupon Code"] = prop;

            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.SecondaryField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Holder4"] = prop;

            prop = new WalletItemCustomProperty("Valid Until", "2014-11-16");
            prop.DetailViewPosition = WalletDetailViewPosition.FooterField1;
            prop.AutoDetectLinks = true;
            prop.SummaryViewPosition = WalletSummaryViewPosition.Field1;
            card.DisplayProperties["Website"] = prop;

            prop = new WalletItemCustomProperty("Website", "http://www.Contoso.com/");
            prop.DetailViewPosition = WalletDetailViewPosition.FooterField2;
            prop.AutoDetectLinks = true;
            prop.SummaryViewPosition = WalletSummaryViewPosition.Field2;
            card.DisplayProperties["Website"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("Phone", "425-707-1234");
            prop.DetailViewPosition = WalletDetailViewPosition.FooterField3;
            prop.AutoDetectLinks = true;
            card.DisplayProperties["phone"] = prop;

            //prop = new WalletItemCustomProperty("Location", "1 Microsot Way, Redmond,  WA 98052");
            //prop.DetailViewPosition = WalletDetailViewPosition.FooterField4;
            //card.DisplayProperties["Address"] = prop;

            // Encode the user's account number as a Qr Code to be used in the store.
            card.Barcode = new WalletBarcode(WalletBarcodeSymbology.Ean13, "9876543210123");

            //card.ExpirationDate = new DateTimeOffset(new DateTime(2015, 1, 1), new TimeSpan(10, 0, 0, 0));

            card.ExpirationDate = new DateTimeOffset(new DateTime(2014, 11, 16));
            //card.ExpirationDate = new DateTimeOffset(DateTime.UtcNow, new TimeSpan(0, 0, 0, 0));

            // NOTE: in the back of the card
            card.LastUpdated = new DateTimeOffset(new DateTime(2014, 10, 10));


            card.IsAcknowledged = true;
            card.IssuerDisplayName = "Contoso Ltd.";
            card.RelevantLocations.Add(
                "Store",
                new WalletRelevantLocation()
                {
                    DisplayMessage = "Store location",
                    Position = new Windows.Devices.Geolocation.BasicGeoposition()
                    {
                        Latitude = 47.640068,
                        Longitude = -122.129858
                    }
                });

            card.Verbs.Add("visit", new WalletVerb("Visit"));


            // Add a relevant date.
            card.RelevantDate = DateTime.Now;
            card.RelevantDateDisplayMessage = "Deal is available all the way in 2014!";

            // Add a promotional message to the card.
            card.DisplayMessage = "Tap here for your 15% off coupon";
            card.IsDisplayMessageLaunchable = true;

            ((Button)sender).IsEnabled = false;
            this.ExpiredDealPassInfoButton.IsEnabled = true;

            await store.AddAsync(expiredDealName, card);
        }

        private async void ExpiredDealPassInfoButton_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();
            await store.ShowAsync(expiredDealName);
        }

        private async void ClearDealsButton_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();
            this.AddDealPassButton.IsEnabled = true;
            this.DealPassInfoButton.IsEnabled = false;

            if (await (store.GetWalletItemAsync(dealName)) != null)
            {
                await store.DeleteAsync(dealName);
            }
            this.AddExpiredDealPassButton.IsEnabled = true;
            this.ExpiredDealPassInfoButton.IsEnabled = false;

            if (await (store.GetWalletItemAsync(expiredDealName)) != null)
            {
                await store.DeleteAsync(expiredDealName);
            }
        }

        #endregion

        #region Boarding Pass
        private async void AddBoardingPassButtonBarCode_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();

            //string itemId = string.IsNullOrEmpty(walletItemName.Text) ? walletItemName.Text : cardName;

            WalletItem card = new WalletItem(WalletItemKind.BoardingPass, airlineName);
            // Set colors, to give the card our distinct branding.
            card.BodyColor = Windows.UI.Colors.Brown;
            card.BodyFontColor = Windows.UI.Colors.White;
            card.HeaderColor = Windows.UI.Colors.SaddleBrown;
            card.HeaderFontColor = Windows.UI.Colors.White;

            //card.bodybackgroundimage = await windows.storage.storagefile.getfilefromapplicationuriasync(new uri("ms-appx:///assets/icons/flight-poster1.jpg"));

            card.Logo336x336 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/airlineMedium.png"));
            card.Logo159x159 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/airlineSmall.png"));
            card.Logo99x99 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/airlineIcon.png"));

            card.LogoImage = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/airlineIcon.png"));
            card.LogoText = airlineName;

            // Add the customer account number.
            WalletItemCustomProperty prop = new WalletItemCustomProperty("Printed date", "2014-12-15");
            prop.DetailViewPosition = WalletDetailViewPosition.HeaderField1;
            card.DisplayProperties["PrintedDate"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.HeaderField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["Holder1"] = prop;


            prop = new WalletItemCustomProperty("Departing from", "Seattle-Tacoma (SEA)");
            prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField1;
            card.DisplayProperties["Origin"] = prop;

            prop = new WalletItemCustomProperty("Destination", "New York (JFK)");
            prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField2;
            prop.AutoDetectLinks = true;
            card.DisplayProperties["Destination"] = prop;

            prop = new WalletItemCustomProperty("Departure Time", "2014-12-19");
            prop.DetailViewPosition = WalletDetailViewPosition.SecondaryField1;
            prop.AutoDetectLinks = true;
            card.DisplayProperties["Departure"] = prop;

            prop = new WalletItemCustomProperty("Arrival Time ", "2014-12-20");
            prop.DetailViewPosition = WalletDetailViewPosition.SecondaryField2;
            prop.AutoDetectLinks = true;
            card.DisplayProperties["Arrival"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.SecondaryField3;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["Holder2"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.SecondaryField4;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["Holder3"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.CenterField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["Holder4"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("Passenger", "Joe Smith");
            prop.DetailViewPosition = WalletDetailViewPosition.FooterField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["Passenger"] = prop;

            prop = new WalletItemCustomProperty("Seat", "12B");
            prop.DetailViewPosition = WalletDetailViewPosition.FooterField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["Seat"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("TicketNumber", "9876543210123");
            prop.DetailViewPosition = WalletDetailViewPosition.FooterField3;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["TicketNumber"] = prop;

            // Encode the user's account number as a Qr Code to be used in the store.
            card.Barcode = new WalletBarcode(WalletBarcodeSymbology.Ean13, "9876543210123");

            //card.ExpirationDate = new DateTimeOffset(new DateTime(2015, 1, 1), new TimeSpan(10, 0, 0, 0));

            card.IsAcknowledged = true;
            card.IssuerDisplayName = "Antarctica Airline";

            card.Verbs.Add("fly", new WalletVerb("Fly"));

            // Add a relevant date.
            card.RelevantDate = DateTime.Now;
            card.RelevantDateDisplayMessage = "Departing soon...";

            // Add a promotional message to the card.
            card.DisplayMessage = "Airline ticket";
            card.IsDisplayMessageLaunchable = true;

            ((Button)sender).IsEnabled = false;
            this.BoardingPassInfoButtonBarcode.IsEnabled = true;

            await store.AddAsync(airlineName, card);
        }

        private async void BoardingPassInfoButtonBarcode_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();
            await store.ShowAsync(airlineName);
        }

        private async void ClearBoardingPassButton_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();
            this.AddBoardingPassButtonBarCode.IsEnabled = true;
            this.BoardingPassInfoButtonBarcode.IsEnabled = false;

            if (await (store.GetWalletItemAsync(airlineName)) != null)
            {
                await store.DeleteAsync(airlineName);
            }
        }
#endregion

        #region Tickets

        private async void AddTicketButtonBarCode_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();

            //string itemId = string.IsNullOrEmpty(walletItemName.Text) ? walletItemName.Text : cardName;

            WalletItem card = new WalletItem(WalletItemKind.Ticket, concertName);
            // Set colors, to give the card our distinct branding.
            card.BodyColor = Windows.UI.Colors.Brown;
            card.BodyFontColor = Windows.UI.Colors.White;
            card.HeaderColor = Windows.UI.Colors.SaddleBrown;
            card.HeaderFontColor = Windows.UI.Colors.White;

            card.Logo336x336 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/wlr_concert_Medium.png"));
            card.Logo159x159 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/wlr_concert_Small.png"));
            card.Logo99x99 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/wlr_concert_Icon.png"));

            card.LogoImage = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/wlr_concert_Icon.png"));
            card.LogoText = concertName;

            // Add the customer account number.
            WalletItemCustomProperty prop = new WalletItemCustomProperty("Printed date", "2014-12-15");
            prop.DetailViewPosition = WalletDetailViewPosition.HeaderField1;
            card.DisplayProperties["PrintedDate"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.HeaderField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["Holder1"] = prop;


            prop = new WalletItemCustomProperty("Departing from", "Seattle-Tacoma (SEA)");
            prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField1;
            card.DisplayProperties["Origin"] = prop;

            prop = new WalletItemCustomProperty("Destination", "New York (JFK)");
            prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField2;
            prop.AutoDetectLinks = true;
            card.DisplayProperties["Destination"] = prop;

            prop = new WalletItemCustomProperty("Departure Time", "2014-12-19");
            prop.DetailViewPosition = WalletDetailViewPosition.SecondaryField1;
            prop.AutoDetectLinks = true;
            card.DisplayProperties["Departure"] = prop;

            prop = new WalletItemCustomProperty("Arrival Time ", "2014-12-20");
            prop.DetailViewPosition = WalletDetailViewPosition.SecondaryField2;
            prop.AutoDetectLinks = true;
            card.DisplayProperties["Arrival"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.SecondaryField3;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["Holder2"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.SecondaryField4;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["Holder3"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.CenterField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["Holder4"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("Name", "Joe Smith");
            prop.DetailViewPosition = WalletDetailViewPosition.FooterField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["Name"] = prop;

            prop = new WalletItemCustomProperty("Seat", "12B");
            prop.DetailViewPosition = WalletDetailViewPosition.FooterField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["Seat"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("TicketNumber", "9876543210123");
            prop.DetailViewPosition = WalletDetailViewPosition.FooterField3;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["TicketNumber"] = prop;

            // Encode the user's account number as a Qr Code to be used in the store.
            card.Barcode = new WalletBarcode(WalletBarcodeSymbology.Ean13, "9876543210123");

            //card.ExpirationDate = new DateTimeOffset(new DateTime(2015, 1, 1), new TimeSpan(10, 0, 0, 0));

            card.IsAcknowledged = true;
            card.IssuerDisplayName = "WLR Concert";

            card.Verbs.Add("see", new WalletVerb("See"));

            // Add a relevant date.
            card.RelevantDate = DateTime.Now;
            card.RelevantDateDisplayMessage = "Once in a lifetime";

            // Add a promotional message to the card.
            card.DisplayMessage = "Concert ticket";
            card.IsDisplayMessageLaunchable = true;

            ((Button)sender).IsEnabled = false;
            this.TicketInfoButtonBarcode.IsEnabled = true;

            await store.AddAsync(concertName, card);
        }

        private async void TicketInfoButtonBarcode_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();
            await store.ShowAsync(concertName);
        }

        private async void ClearTicketButton_Click(object sender, RoutedEventArgs e)
        {

            WalletItemStore store = await WalletManager.RequestStoreAsync();

            this.AddTicketButtonBarCode.IsEnabled = true;
            this.TicketInfoButtonBarcode.IsEnabled = false;

            if (await (store.GetWalletItemAsync(concertName)) != null)
            {
                await store.DeleteAsync(concertName);
            }
        }
#endregion

        #region App Bar
        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            //Uri uri = new Uri("app://5B04B775-356B-4AA0-AAF8-6491FFEA5633/ExternalMarketplaceRequest?page=MarketplaceTagSearch&tag=wallet_card_payment&tag=wallet_app_transit&tag=wallet_app_loyalty&tag=wallet_app_membership&tag=wallet_app_other");
            Uri uri = new Uri("ms-windows-store:search?keyword=wallet&contenttype=app");
            ////Uri uri = new Uri("ms-windows-store:search?tag=wallet_app_loyalty&contenttype=app");
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);

            if (success)
            {
                // URI launched
            }
            else
            {
                // URI launch failed
                var messageDialog = new MessageDialog("Cannot launch the store app.");

                // Show the message dialog
                await messageDialog.ShowAsync();
            }
        }

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {


            this.ClearBoardingPassButton_Click(sender, e);
            this.ClearDealsButton_Click(sender, e);
            this.ClearTicketButton_Click(sender, e);
            this.ClearMembershipButton_Click(sender, e);
            this.ClearOthersButton_Click(sender, e);

            WalletItemStore store = await WalletManager.RequestStoreAsync();
            await store.ClearAsync();
        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            if (this.AddBoardingPassButtonBarCode.IsEnabled)
            {
                this.AddBoardingPassButtonBarCode_Click(this.AddBoardingPassButtonBarCode, e);
            }
            if (this.AddDealPassButton.IsEnabled)
            {
                this.AddDealPassButtonBarCode_Click(this.AddDealPassButton, e);
            }
            if (this.AddExpiredDealPassButton.IsEnabled)
            {
                this.AddExpiredDealPassButton_Click(this.AddExpiredDealPassButton, e);
            }
            if (this.AddMembershipButtonBarCode.IsEnabled)
            {
                this.AddMembershipButtonBarcode_Click(this.AddMembershipButtonBarCode, e);
            }
            if (this.AddMembershipButtonQR.IsEnabled)
            {
                this.AddMembershipButtonQR_Click(this.AddMembershipButtonQR, e);
            }
            if (this.AddTicketButtonBarCode.IsEnabled)
            {
                this.AddTicketButtonBarCode_Click(this.AddTicketButtonBarCode, e);
            }
            if (this.AddPIButtonQR.IsEnabled)
            {
                this.AddPIButtonQR_Click(this.AddPIButtonQR, e);
            }
            if (this.AddGeneralButtonQR.IsEnabled)
            {
                this.AddGeneralButtonQR_Click(this.AddGeneralButtonQR, e);
            }
        }


        private async void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();
            await store.ShowAsync();
        }

        #endregion

        #region Payment Instruments
        private async void AddPIButtonQR_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();

            //string itemId = string.IsNullOrEmpty(walletItemName.Text) ? walletItemName.Text : cardName;

            WalletItem card = new WalletItem(WalletItemKind.PaymentInstrument, paymentInstrumentName);
            // Set colors, to give the card our distinct branding.
            card.BodyColor = Windows.UI.Colors.Brown;
            card.BodyFontColor = Windows.UI.Colors.White;
            card.HeaderColor = Windows.UI.Colors.SaddleBrown;
            card.HeaderFontColor = Windows.UI.Colors.White;


            card.Logo336x336 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/litecoin_Medium.png"));
            card.Logo159x159 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/litecoin_Small.png"));
            card.Logo99x99 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/litecoin_Icon.png"));

            card.LogoImage = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/litecoin_Icon.png"));
            card.LogoText = paymentInstrumentName;

            // Add the customer account number.
            WalletItemCustomProperty prop = new WalletItemCustomProperty("Litecoin", "Litecoin");
            prop.DetailViewPosition = WalletDetailViewPosition.HeaderField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Title"] = prop;

            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.HeaderField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Holder2"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("HOLD", "Hold");
            prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Hold"] = prop;

            prop = new WalletItemCustomProperty("Website", "https://litecoin.org/");
            prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField2;
            card.DisplayProperties["Website"] = prop;

          

            prop = new WalletItemCustomProperty("Balance", "99");
            prop.DetailViewPosition = WalletDetailViewPosition.FooterField1;
            prop.SummaryViewPosition = WalletSummaryViewPosition.Field1;
            card.DisplayProperties["Points"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("Account Number", "12345678");
            prop.DetailViewPosition = WalletDetailViewPosition.FooterField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["AcctId"] = prop;

            // Encode the user's account number as a Qr Code to be used in the store.
            card.Barcode = new WalletBarcode(WalletBarcodeSymbology.Qr, "12345678");

            // Add a promotional message to the card.
            card.DisplayMessage = "Try out Litecoin now";
            card.IsDisplayMessageLaunchable = true;

            ((Button)sender).IsEnabled = false;
            this.PIInfoButtonQR.IsEnabled = true;

            await store.AddAsync(paymentInstrumentName, card);
        }

        private async void PIInfoButtonQR_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();
            await store.ShowAsync(paymentInstrumentName);
        }

        private async void ClearPIButton_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();

            this.AddPIButtonQR.IsEnabled = true;
            this.PIInfoButtonQR.IsEnabled = false;

            if (await (store.GetWalletItemAsync(concertName)) != null)
            {
                await store.DeleteAsync(concertName);
            }
        }

        #endregion

        #region Others
        private async void AddGeneralButtonQR_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();

            //string itemId = string.IsNullOrEmpty(walletItemName.Text) ? walletItemName.Text : cardName;

            WalletItem card = new WalletItem(WalletItemKind.General, generalItemName);
            // Set colors, to give the card our distinct branding.
            card.BodyColor = Windows.UI.Colors.Brown;
            card.BodyFontColor = Windows.UI.Colors.White;
            card.HeaderColor = Windows.UI.Colors.SaddleBrown;
            card.HeaderFontColor = Windows.UI.Colors.White;


            card.Logo336x336 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/stellarMedium.png"));
            card.Logo159x159 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/stellarSmall.png"));
            card.Logo99x99 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/stellarIcon.png"));

            card.LogoImage = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/icons/stellarIcon.png"));
            card.LogoText = generalItemName;

            // Add the customer account number.
            WalletItemCustomProperty prop = new WalletItemCustomProperty("Litecoin", "Litecoin");
            prop.DetailViewPosition = WalletDetailViewPosition.HeaderField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Title"] = prop;

            prop = new WalletItemCustomProperty("HOLD", "HOLD");
            prop.DetailViewPosition = WalletDetailViewPosition.HeaderField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Holder2"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("HOLD", "Hold");
            prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField1;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            card.DisplayProperties["Hold"] = prop;

            prop = new WalletItemCustomProperty("Website", "https://www.msn.com/");
            prop.DetailViewPosition = WalletDetailViewPosition.PrimaryField2;
            card.DisplayProperties["Website"] = prop;



            prop = new WalletItemCustomProperty("Balance", "0");
            prop.DetailViewPosition = WalletDetailViewPosition.FooterField1;
            prop.SummaryViewPosition = WalletSummaryViewPosition.Field1;
            card.DisplayProperties["Points"] = prop;

            // Add the customer account number.
            prop = new WalletItemCustomProperty("Account Number", "12345678");
            prop.DetailViewPosition = WalletDetailViewPosition.FooterField2;
            // We don't want this field entity extracted as it will be interpreted as a phone number.
            prop.AutoDetectLinks = false;
            card.DisplayProperties["AcctId"] = prop;

            // Encode the user's account number as a Qr Code to be used in the store.
            //card.Barcode = new WalletBarcode(WalletBarcodeSymbology.Qr, "12345678");

            // Add a promotional message to the card.
            card.DisplayMessage = "MSN!";
            card.IsDisplayMessageLaunchable = true;

            ((Button)sender).IsEnabled = false;
            this.GeneralInfoButtonQR.IsEnabled = true;

            await store.AddAsync(generalItemName, card);
        }

        private async void GeneralInfoButtonQR_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();
            await store.ShowAsync(generalItemName);
        }

        private async void ClearOthersButton_Click(object sender, RoutedEventArgs e)
        {
            WalletItemStore store = await WalletManager.RequestStoreAsync();

            this.AddGeneralButtonQR.IsEnabled = true;
            this.GeneralInfoButtonQR.IsEnabled = false;
            if (await (store.GetWalletItemAsync(generalItemName)) != null)
            {
                await store.DeleteAsync(generalItemName);
            }
        }
        #endregion

    }
}
