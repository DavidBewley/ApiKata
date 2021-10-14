# Employee Api

This is a simple API to interface with for teaching purposes. It records employee's name's and start dates and allows CRUD operations to be performed at the following endpoints:

| HTTP Request			| Functionality 				| Route 			|
| ------------- 		| ---------------------------	| ------------- 	|
| GET  					| Retrieve employee info  		| /employees/{id}	|
| POST  				| Add a new employee  			| /employees		|
| PUT  					| Update an employee  			| /employees		|
| DELETE  				| Delete an employee  			| /employees/{id}	|

The POST  and PUT requests should take a body containing the following as JSON:

```JSON
	{
		"id" : "00000000-0000-0000-0000-000000000000",
		"Name" : "Joe Bloggs",
		"StartDate" : "2021-01-01T00:00:00.0000000+00:00"
	}
```