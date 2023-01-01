# dotnet-api-request-challenge
A WebApi exercise for csharp

A user can make an api call to this service which then in turn makes an api call to IPStack
to retrieve information about the requested IP. The IP information is then stored in a 
Microsoft SQL database, and cached for a limited amount of time for any further retrievals 
of the same IP. 

The user can also make a batch PATCH request to update any information about the IPs they choose
which is handled by a background service queue system.
