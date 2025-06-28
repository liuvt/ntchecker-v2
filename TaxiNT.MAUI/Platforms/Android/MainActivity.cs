using Android.App;
using Android.Content.PM;
using Android.OS;
using static Android.Graphics.Paint;
using static KotlinX.AtomicFU.TraceBase;

namespace TaxiNT.MAUI
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
    }
}


//keytool -genkeypair -v -keystore my.keystore -alias TaxiNTKeyName -keyalg RSA -keysize 2048 -validity 10000
//Password: 110894
/*
    What is your first and last name ?
      [Unknown] : LuuVan
    What is the name of your organizational unit?
      [Unknown]:  None
    What is the name of your organization?
      [Unknown]:  None
    What is the name of your City or Locality?
      [Unknown]:  CanTho
    What is the name of your State or Province?
      [Unknown]:  VietNam
    What is the two-letter country code for this unit?
      [Unknown]:  97
*/

//keytool -list -keystore TaxiNTMAUI.keystore

//dotnet publish -f net9.0-android -c Release -p:AndroidKeyStore=true -p:AndroidSigningKeyStore=my.keystore -p:AndroidSigningKeyAlias=TaxiNTKeyName -p:AndroidSigningKeyPass=110894 -p:AndroidSigningStorePass=110894