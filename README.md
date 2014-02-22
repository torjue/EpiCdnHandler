# EpiCdnHandler

Customer origin CDN support for EpiServer 7.5 (or newer).

Reference this project from your main EpiServer web project (NuGet package coming soon).

The module will rewrite and handle all image urls. It will add a version hash to the urls. The module will set http headers on the requests to "permanently" cache the image files on the client.

Example:

    http://example.com/globalassets/image.jpg
	to
	http://example.com/cdn-a3c5b10e/globalassets/image.jpg

The module will work without any configuration, but if you want to set your own base url (to your cdn provider) or disable the module, you will need to add the following to your web.config:

    <configuration>
        <configSections>
        ....
        <section name="epiCdnHandler" type="EpiCdnHandler.CdnConfigurationSection, EpiCdnHandler" />
    </configSections>
    <epiCdnHandler enabled="true" url="http://your-cdn-url" />


### License
MIT: [torjue.mit-license.org](http://torjue.mit-license.org)