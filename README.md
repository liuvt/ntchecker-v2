# Bill Checker v2
- Blazor Static
- Blazor Server
- Blazor Webassembly component
- Mobile: MAUI Blazor Hybrid App

# GoogleSheet Databases Extension
- Create project: [Google Cloud Console - Service Accounts](https://console.cloud.google.com/iam-admin/serviceaccounts)
- Register service
- Create API
- Down file Json account, save name: ```ggsheetaccount.json```
  
# Tailwind layout : https://tailwindcss.com/docs/installation/tailwind-cli
- Local run cmd: ../ntchecker-v2
- Install Tailwind CSS: ```npm install tailwindcss @tailwindcss/cli```
- Build process for WEB: ```npx @tailwindcss/cli -i ./TaxiNT/TailwindImport/input.css -o ./TaxiNT/wwwroot/css/tailwindcss.css --watch```
- Build process for Mobile: ```npx @tailwindcss/cli -i ./TaxiNT.MAUI/TailwindImport/input.css -o ./TaxiNT.MAUI/wwwroot/css/tailwindcss.css --watch```
