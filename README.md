Jarboo.Websites.Admin
=====================

This was my internal admin system when I ran the consulting business, Jarboo.com. I had a lot of developers I used for outsourcing, and we used this tool to manage and update projects.


Trello integration
------------

1. [Log in](https://trello.com/login)
2. [Retrieve API key](https://trello.com/app-key)
3. Using API key request token with next url:
  * https://trello.com/1/authorize?key=API_KEY&name=Jarboo.Admin&expiration=never&response_type=token&scope=read,write
4. Set [web.config's](https://github.com/LarsHoldgaard/Jarboo.Websites.Admin/blob/master/Code/Jarboo.Admin.Web/Web.config) **TrelloApiKey** and **TrelloToken** app settings
5. Integration complete

Google Drive integration
------------

1. Enable the Drive API
 1. Go to the [Google Developers Console](https://console.developers.google.com/)
 2. Select a project, or create a new one
 3. In the sidebar on the left, expand **APIs & auth**. Next, click **APIs**. In the list of APIs, make sure the status is **ON** for the Drive API.
2. Register client
 1. In your project menu on the left, select **APIs & auth / Credentials**
 2. Press **Create new Client ID**
 3. Select **Web application**
 4. Fill consent info
 5. Authorized JavaScript origins: *SITE_URL* (e.g. http://localhost:8000)
 6. Authorized redirect URIs: *SITE_URL/AuthCallback/IndexAsync*  (e.g. http://localhost:8000/AuthCallback/IndexAsync)
 7. Set [web.config's](https://github.com/LarsHoldgaard/Jarboo.Websites.Admin/blob/master/Code/Jarboo.Admin.Web/Web.config) **GoogleClientId** and **GoogleClientSecret** app settings
3. Request refresh token
 1. Log in google account with Drive
 2. Open web application and go to *SITE_URL/admin/requestrefreshtoken* (e.g. http://localhost:8000/admin/requestrefreshtoken)
 3. Permit access for application to the Google Drive
 4. Copy refresh token to [web.config's](https://github.com/LarsHoldgaard/Jarboo.Websites.Admin/blob/master/Code/Jarboo.Admin.Web/Web.config) **GoogleRefreshToken** app setting
4. Integration complete

In order to rerequest refresh token you need to disconnect application from Drive first.
