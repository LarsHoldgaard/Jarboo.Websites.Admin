Jarboo.Websites.Admin
=====================

Trello integration
------------

1. [Log in](https://trello.com/login)
2. [Retrieve API key](https://trello.com/app-key)
3. Using API key request token with next url:
  * https://trello.com/1/authorize?key=API_KEY&name=Jarboo.Admin&expiration=never&response_type=token&scope=read,write
4. Set [web.config's](https://github.com/LarsHoldgaard/Jarboo.Websites.Admin/blob/master/Code/Jarboo.Admin.Web/Web.config) **TrelloApiKey** and **TrelloToken** app settings.
