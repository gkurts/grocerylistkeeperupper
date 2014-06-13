grocerylistkeeperupper
======================

Just a playground for trying new things and seeing what I can break / fix.

### contains:
* NancyFx based API
* Nancy.Authentication.Token for authentication
* Dapper.Net for a clean and **fast** ORM
* SQL Server based, though I'm looking at options for a document based data store. Maybe RavenDb or MongoDb.
* AngularJS / Bootstrap based web interface


### To do
* general code cleanup 
 * separate classes into individual files
 * angular javascript stuff into individual files
* implement "features" structure in both api and web interface (http://the.fringe.ninja/blog/481/organizing-per-feature-in-nancy)
* implement a task runner to minify and concatenate the javascript files into one dist file (or something like that)
* javascript linter
* registration screen does not give feedback at all. Should automatically login and redirect.
* login page doesn't give feedback on bad password.
* add a logout option
* persist token to local cookie
* android version
* iphone version


### check it out
* api at  http://glkuapi.azurewebsites.net
* web at http://glku.azurewebsites.net
