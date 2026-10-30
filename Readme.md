# MFaaP.MFWSClient (C# M-Files Web Service Wrapper)

*Please note that this library is provided "as-is" and with no warranty, explicit or otherwise.  You should ensure that the functionality meets your requirements, and thoroughly test them, prior to using in any production scenarios.*

The following helper library is provided as work-in-progress, and may not be fully complete.

This library aims to provide an easy-to-use C# wrapper for the [M-Files Web Service](http://www.m-files.com/MFWS/), which is part of the M-Files Web Access.  The user guide contains more information on [setting up M-Files Web Access](http://www.m-files.com/user-guide/latest/eng/#Configure_M-Files_Web_Access.html).

It currently provides the following functionality:

* Authentication, both using credentials and using Windows Single Sign On
* Object creation
* File upload, including the stateless file upload (file references)
* Vault extension method execution
* Searching

## Nuget package

This library is available [via nuget](https://www.nuget.org/packages/MFaaP.MFWSClient).  The simplest way to get started with this library is to add the `MFaaP.MFWSClient` package to your class library.  .NET Framework 4.6 (and higher) and .NET Standard 2.0 runtimes are supported.

## Basic usage

This functionality is exposed by the `MFWSClient` object, which takes the URL of the M-Files Web Service in the constructor:

```csharp
// Instantiate a new MFWS client.
var client = new MFWSClient("http://m-files.mycompany.com");
```

The API provides both "Async" and blocking versions of most methods.  To use .NET Tasks and the async/await pattern, simply use the *Async version of the method (e.g. `AuthenticateUsingCredentialsAsync` instead of `AuthenticateUsingCredentials`).

## Authentication

Currently three methods of authentication are supported: authentication using credentials, and Windows Single Sign On.

## Authenticating using credentials

```csharp
// Instantiate a new MFWS client.
var client = new MFWSClient("http://m-files.mycompany.com");

// Authentiate to a vault with GUID {C840BE1A-5B47-4AC0-8EF7-835C166C8E24} (clear credentials).
client.AuthenticateUsingCredentials
(
    Guid.Parse("{C840BE1A-5B47-4AC0-8EF7-835C166C8E24}"),
    "MyUsername",
    "MyPassword",
	Guid.NewGuid.ToString() // Providing a session ID allows logout.
);
```

Note that the M-Files Web Service will provide authentication tokens even if the credentials are incorrect.  This is by design.

### Automatically expiring an authentication token

If no expiry information is provided, an authentication token will be valid indefinitely.  To set a specific expiry datetime, pass the datetime on the authentication call:

```csharp
// Instantiate a new MFWS client.
var client = new MFWSClient("http://m-files.mycompany.com");

// Authentiate to a vault with GUID {C840BE1A-5B47-4AC0-8EF7-835C166C8E24} (clear credentials).
// Set the expiry as 10am UTC on 1st January 2017.
client.AuthenticateUsingCredentials
(
    Guid.Parse("{C840BE1A-5B47-4AC0-8EF7-835C166C8E24}"),
    "MyUsername",
    "MyPassword",
    new DateTime(2017, 01, 01, 10, 00, 00, DateTimeKind.Utc)
);
```

Alternatively, to expire after a specified time from the initial authentication, provide a TimeSpan when authenticating:

```csharp
// Instantiate a new MFWS client.
var client = new MFWSClient("http://m-files.mycompany.com");

// Authentiate to a vault with GUID {C840BE1A-5B47-4AC0-8EF7-835C166C8E24} (clear credentials).
// Set the expiry as 1 hour from initial authentication.
client.AuthenticateUsingCredentials(
    Guid.Parse("{C840BE1A-5B47-4AC0-8EF7-835C166C8E24}"),
    "MyUsername",
    "MyPassword",
    TimeSpan.FromHours(1) );
```


## Authenticating using Windows Single Sign On

If using Windows Single Sign On, the application will use the current Windows identity that it is running under.  Note that [using Windows Single Sign On requires additional configuration](https://partners.cloudvault.m-files.com/Default.aspx?#CE7643CB-C9BB-4536-8187-707DB78EAF2A/object/75F59ED5-CC7F-4A0A-90D5-0F582D26E884/latest).

```csharp
// Instantiate a new MFWS client.
var client = new MFWSClient("http://m-files.mycompany.com");

// Authentiate to a vault with GUID {C840BE1A-5B47-4AC0-8EF7-835C166C8E24}.
client.AuthenticateUsingSingleSignOn( Guid.Parse("{C840BE1A-5B47-4AC0-8EF7-835C166C8E24}") );
```

## Authenticating using OAuth 2.0

Connecting using OAuth 2.0 requires several steps:

1. Call `MFWSClient.GetOAuth2Configuration`.  If this returns null then OAuth 2.0 is not configured.
2. Open a web browser and navigate to `OAuth2Configuration.GenerateAuthorizationUri`.
3. Handle any redirects that occur within the web browser.  If the redirected URI **does not** start with `OAuth2Configuration.GetAppropriateRedirectUri` then allow it to occur.
4. For redirects that **do** start with `OAuth2Configuration.GetAppropriateRedirectUri`, call `MFWSClient.GenerateTokensFromOAuthRedirect`.  Note that this may throw an exception if the login failed.

## Searching

### Quick search

Simple searching can be done using the `SearchForObjectsByString` method.

Note that the search will only return items which you have access to, so ensure that you are authenticated (if required) prior to executing the method.

```csharp
// Connect to the online knowledgebase.
// Note that this doesn't require authentication.
var client = new MFWSClient("http://kb.cloudvault.m-files.com");

// Execute a simple search for the word "mfws".
var results = client.ObjectSearchOperations.SearchForObjectsByString("mfws");

// Iterate over the results and output them.
System.Console.WriteLine($"There were {results.Length} results returned.");
foreach (var objectVersion in results)
{
	System.Console.WriteLine($"\t{objectVersion.Title}");
	System.Console.WriteLine($"\t\tType: {objectVersion.ObjVer.Type}, ID: {objectVersion.ObjVer.ID}");
}
```

### Advanced / complex search

In addition to a simple quick search, the helper library allows the execution of more complex, or "advanced" searches using the `Search` method.  This method accepts a collection of `ISearchCondition` objects which can be used to further constrain any search.

Note that the search will only return items which you have access to, so ensure that you are authenticated (if required) prior to executing the method.

```csharp
// Connect to the online knowledgebase.
// Note that this doesn't require authentication.
var client = new MFWSClient("http://kb.cloudvault.m-files.com");

// Execute an advanced search for the word "mfws", restricted to object type 0 (documents), which have a Document Date (property 1002) greater than 2015-11-01.
var results = client.ObjectSearchOperations.SearchForObjectsByConditions(
	new QuickSearchCondition("mfws"),
	new ObjectTypeSearchCondition(0),
	new DatePropertyValueSearchCondition(1002, new DateTime(2015, 11, 01), SearchConditionOperators.GreaterThan)
);

// Iterate over the results and output them.
System.Console.WriteLine($"There were {results.Length} results returned.");
foreach (var objectVersion in results)
{
	System.Console.WriteLine($"\t{objectVersion.Title}");
	System.Console.WriteLine($"\t\tType: {objectVersion.ObjVer.Type}, ID: {objectVersion.ObjVer.ID}");
}
```



#### Restricting by text properties

For example, to restrict the search results by the value of a property with ID `1002`:

*Note that the library will handle ensuring items are correctly encoded.**

```csharp
// Create our search condition.
var condition = new TextPropertyValueSearchCondition(1002, "ESTT");
```

If we wished to restrict by a text property with ID where the field contains `ESTT`, we would alter the operator:

```csharp
// Create our search condition.
var condition = new TextPropertyValueSearchCondition(1002, "ESTT", SearchConditionOperators.Contains);
```

If we wished to restrict by a text property with ID where the field matches a [wildcard search for `ESTT*`](http://www.m-files.com/user-guide/latest/eng/#Quick_search.html), we would alter the operator:

```csharp
// Create our search condition.
var condition = new TextPropertyValueSearchCondition(1002, "ESTT", SearchConditionOperators.MatchesWildcard);
```

#### Restricting by a boolean property

To restrict the search results by the value of a boolean property with ID `1050`:

```csharp
// Create our search condition.
var condition = new BooleanPropertyValueSearchCondition(1050, true);
```

Alternatively, we could search for only objects where the property is false:

```csharp
// Create our search condition.
var condition = new BooleanPropertyValueSearchCondition(1050, false);
```

#### Restricting by a numeric property (integer or real)

To restrict the search results by the value of a numeric property with ID `1100`:

```csharp
// Create our search condition.
var condition = new NumericPropertyValueSearchCondition(1100, 123);
```

Alternatively, we could make use of a [different operator](#operators) to instead search for objects where the value is greater than 1000:

```csharp
// Create our search condition.
var condition = new NumericPropertyValueSearchCondition(1100, 1000, SearchConditionOperators.GreaterThan);
```

#### Restricting by a date/time or timestamp property

To restrict the search results by the value of a date property with ID `1200`:

```csharp
// Create our search condition.
var condition = new DatePropertyValueSearchCondition(1200, new DateTime(2017, 6, 1));
```

Alternatively, we could make use of a [different operator](#operators) to instead search for objects where the date is newer than 1st June 2017:

```csharp
// Create our search condition.
var condition = new DatePropertyValueSearchCondition(1200, new DateTime(2017, 6, 1), SearchConditionOperators.GreaterThan);
```

#### Restricting by a lookup (single-select) property

To restrict the search results by the value of a single-select lookup property with ID `1500` that reference an object or value list item with ID 12345:

```csharp
// Create our search condition.
var condition = new LookupPropertyValueSearchCondition(1500, 12345);
```

#### Restricting by a multi-select lookup property

To restrict the search results by the value of a single-select lookup property with ID `1500` that reference an object or value list item with ID 12345:

*If there is only one value then this is the same as a single-select-lookup.*

```csharp
// Create our search condition.
var condition = new MultiSelectLookupPropertyValueSearchCondition(1500, 12345);
```

To restrict objects to only objects which reference objects or value list items with *either* IDs 1, 2, 3, or 4:

```csharp
// Create our search condition.
var condition = new MultiSelectLookupPropertyValueSearchCondition(1500, new [] { 1, 2, 3, 4 });
```

## Executing Vault Extension Methods

[Vault Extension Methods](http://developer.m-files.com/Built-In/VBScript/Vault-Extension-Methods/) are named sections of code, loaded into the M-Files vault, that can be executed using the M-Files API(s).  Vault Extension Methods can be executed using the [M-Files REST API](http://developer.m-files.com/APIs/REST-API/Vault-Extension-Methods/) by executing a correctly-formatted HTTP request.

The wrapper API exposes extension methods in a similar manner as the [COM API](https://www.m-files.com/api/documentation/latest/index.html#MFilesAPI~VaultExtensionMethodOperations~ExecuteVaultExtensionMethod.html):

```csharp
// Instantiate a new MFWS client.
var client = new MFWSClient("http://m-files.mycompany.com");

// Authentiate to a vault with GUID {C840BE1A-5B47-4AC0-8EF7-835C166C8E24} (clear credentials).
client.AuthenticateUsingCredentials(Guid.Parse("{C840BE1A-5B47-4AC0-8EF7-835C166C8E24}"), "MyUsername", "MyPassword")

// Execute an extension method with the name "MyExtensionMethod", passing it the input string of "MyInputValue".
var output = client.ExtensionMethodOperations.ExecuteVaultExtensionMethod("MyExtensionMethod", "MyInputValue");
```

## Creating objects

## Creating a new object

### Without files

This process can be used to create objects of any type (Document or anything else) that do not contain files.  The sample below shows a Document object being created

```csharp
// Instantiate a new MFWS client.
var client = new MFWSClient("http://m-files.mycompany.com");

// Authentiate to a vault with GUID {C840BE1A-5B47-4AC0-8EF7-835C166C8E24} (clear credentials).
client.AuthenticateUsingCredentials(Guid.Parse("{C840BE1A-5B47-4AC0-8EF7-835C166C8E24}"), "MyUsername", "MyPassword")

//Create an ObjectCreationInfo containing the properties of the new object
var newObjectDetails = new ObjectCreationInfo()
{
	// Create the property values for the new object.
	PropertyValues = new[] {
		new PropertyValue() {
			PropertyDef = 100, // Built-in property definition of "class".
			TypedValue = new TypedValue()
			{
				DataType = MFDataType.Lookup,
				Lookup = new Lookup()
				{
					Item = 0, // The id 0 is the default Document class lookup ID
					Version = -1
				}
			}
		},
		new PropertyValue()
		{
			// Property value 22 ("Single File Object") is required when creating a Document Object
			PropertyDef = 22,
			TypedValue = new TypedValue()
			{
				DataType = MFDataType.Boolean,
				Value = false // false = "multi-file-document" (it is not a SINGLE file document as it has zero files)
			}
		},
		new PropertyValue()
		{
			PropertyDef = 0, // Property definition 0 (Name or Title) is the default title property
			TypedValue = new TypedValue()
			{
				DataType = MFDataType.Text,
				Value = "Sample Title"
			}
		}
	}
};

// Create the object in the M-Files vault and return data about the new object.
var newObjectVersion = client.ObjectOperations.CreateNewObject(
						0, // 0 is the built-in object type of "Document"
						newObjectDetails);
```

### With files

Upload the files first with `ObjectFileOperations.UploadFiles`, then pass the returned upload information as the `Files` of the new object.  The upload information is passed on as it is returned: it identifies the uploaded content and, for a stateless upload, carries its checksum.

```csharp
// Instantiate a new MFWS client.
var client = new MFWSClient("http://m-files.mycompany.com");

// Authentiate to a vault with GUID {C840BE1A-5B47-4AC0-8EF7-835C166C8E24} (clear credentials).
client.AuthenticateUsingCredentials(Guid.Parse("{C840BE1A-5B47-4AC0-8EF7-835C166C8E24}"), "MyUsername", "MyPassword");

// Upload the file.  The server returns a reference to the uploaded content.
var uploads = client.ObjectFileOperations.UploadFiles(new FileInfo(@"C:\Temp\Report.pdf"));

// Create a single-file document that has the uploaded file as its content.
var newObjectDetails = new ObjectCreationInfo()
{
	PropertyValues = new[] {
		new PropertyValue()
		{
			PropertyDef = 100, // Built-in property definition of "class".
			TypedValue = new TypedValue()
			{
				DataType = MFDataType.Lookup,
				Lookup = new Lookup() { Item = 0, Version = -1 } // The id 0 is the default Document class lookup ID
			}
		},
		new PropertyValue()
		{
			PropertyDef = 22, // Property value 22 ("Single File Object") is required when creating a Document Object
			TypedValue = new TypedValue() { DataType = MFDataType.Boolean, Value = true } // true = "single-file-document"
		},
		new PropertyValue()
		{
			PropertyDef = 0, // Property definition 0 (Name or Title) is the default title property
			TypedValue = new TypedValue() { DataType = MFDataType.Text, Value = "Sample Title" }
		}
	},
	Files = uploads
};

// Create the object in the M-Files vault and return data about the new object.
var newObjectVersion = client.ObjectOperations.CreateNewObject(
						0, // 0 is the built-in object type of "Document"
						newObjectDetails);
```

### Adding and replacing files

Files can be added to an existing object, and the content of an existing file replaced, with `ObjectFileOperations.AddFiles` and `ObjectFileOperations.UploadFile`:

```csharp
// Add a file to the object.
var extendedObjectVersion = client.ObjectFileOperations.AddFiles(objVer, new FileInfo(@"C:\Temp\Another.pdf"));

// Replace the content of a file of the object.
client.ObjectFileOperations.UploadFile(objVer, fileVer, @"C:\Temp\Report.pdf");
```

### The stateless file upload

M-Files servers that support the stateless file upload (the server capability `FileUploadStatelessSupported`, M-Files MFILES_VERSION_TBD and later) identify an upload by a file reference (`UploadInfo.FileRef`) instead of an upload ID (`UploadInfo.UploadID`).  A file reference can be used by any server instance, so the requests of an upload do not need a sticky session (session affinity) to one server.  The upload ID is obsolete.

The library asks for file references by itself:

* It retrieves the capabilities of the server once (`client.GetServerCapabilities()`), and uses file references when the server supports them.  A server that does not support them, or does not return its capabilities, gets upload IDs, as before.
* The uploaded content is committed with its MD5 checksum.  The server returns it when it receives the whole file in one request (`UploadInfo.Checksum`).  When a file is uploaded in blocks, the library calculates it.  Do not change the `FileRef` or the `Checksum` of the returned upload information; `UploadInfo.IsStateless()` tells which kind of upload it is.
* The metadata suggestions of uploaded files are fetched with `AutomaticMetadataOperations.GetAutomaticMetadataForUploads`, which takes the upload information.  The methods that take the ids of temporary files are obsolete, as upload IDs are.

A file reference can be committed only once, so a failed commit needs a new upload.

## Checking an object in and out.

```csharp
// Instantiate a new MFWS client.
var client = new MFWSClient("http://m-files.mycompany.com");

// Authentiate to a vault with GUID {C840BE1A-5B47-4AC0-8EF7-835C166C8E24} (clear credentials).
client.AuthenticateUsingCredentials(Guid.Parse("{C840BE1A-5B47-4AC0-8EF7-835C166C8E24}"), "MyUsername", "MyPassword")

// Check out the document (type ID 0) with ID 567.
var objID = new MFaaP.MFWSClient.ObjID()
{
	Type = 0,
	ID = 567
};
var obj = client.ObjectOperations.CheckOut(objID);

// Make a change to the object.
client.ObjectPropertyOperations.SetProperty(obj.ObjVer, new PropertyValue()
{
	PropertyDef = 1088, // The property ID 1088.
	TypedValue = new MFaaP.MFWSClient.TypedValue()
	{
		DataType = MFDataType.Text,
		Value = "hello world"
	}
});

// Check the object back in.
client.ObjectOperations.CheckIn(obj.ObjVer);
```

### Undoing a checkout

```csharp
// Instantiate a new MFWS client.
var client = new MFWSClient("http://m-files.mycompany.com");

// Authentiate to a vault with GUID {C840BE1A-5B47-4AC0-8EF7-835C166C8E24} (clear credentials).
client.AuthenticateUsingCredentials(Guid.Parse("{C840BE1A-5B47-4AC0-8EF7-835C166C8E24}"), "MyUsername", "MyPassword")

// Check out the document (type ID 0) with ID 567.
var objID = new MFaaP.MFWSClient.ObjID()
{
	Type = 0,
	ID = 567
};
var obj = client.ObjectOperations.CheckOut(objID);

// Undo the checkout.
client.ObjectOperations.UndoCheckout(obj.ObjVer);
```

## Modifying object properties

### Updating properties

```csharp
// Instantiate a new MFWS client.
var client = new MFWSClient("http://m-files.mycompany.com");

// Authentiate to a vault with GUID {C840BE1A-5B47-4AC0-8EF7-835C166C8E24} (clear credentials).
client.AuthenticateUsingCredentials(Guid.Parse("{C840BE1A-5B47-4AC0-8EF7-835C166C8E24}"), "MyUsername", "MyPassword")

// Check out the document (type ID 0) with ID 567.
var objID = new MFaaP.MFWSClient.ObjID()
{
	Type = 0,
	ID = 567
};
var obj = client.ObjectOperations.CheckOut(objID);

// Update a property on the object.
client.ObjectPropertyOperations.SetProperty(obj.ObjVer, new PropertyValue()
{
	PropertyDef = 1088, // The property ID 1088.
	TypedValue = new MFaaP.MFWSClient.TypedValue()
	{
		DataType = MFDataType.Text,
		Value = "hello world"
	}
});

// Check the object back in.
client.ObjectOperations.CheckIn(obj.ObjVer);
```

### Removing properties

```csharp
// Instantiate a new MFWS client.
var client = new MFWSClient("http://m-files.mycompany.com");

// Authentiate to a vault with GUID {C840BE1A-5B47-4AC0-8EF7-835C166C8E24} (clear credentials).
client.AuthenticateUsingCredentials(Guid.Parse("{C840BE1A-5B47-4AC0-8EF7-835C166C8E24}"), "MyUsername", "MyPassword")

// Check out the document (type ID 0) with ID 567.
var objID = new MFaaP.MFWSClient.ObjID()
{
	Type = 0,
	ID = 567
};
var obj = client.ObjectOperations.CheckOut(objID);

// Remove the property from the object.
client.ObjectPropertyOperations.RemoveProperty(obj.ObjVer, 1088);

// Check the object back in.
client.ObjectOperations.CheckIn(obj.ObjVer);
```

## Deleting and undeleting an object

### Deleting

```csharp
// Instantiate a new MFWS client.
var client = new MFWSClient("http://m-files.mycompany.com");

// Authentiate to a vault with GUID {C840BE1A-5B47-4AC0-8EF7-835C166C8E24} (clear credentials).
client.AuthenticateUsingCredentials(Guid.Parse("{C840BE1A-5B47-4AC0-8EF7-835C166C8E24}"), "MyUsername", "MyPassword")

// Delete the document (type ID 0) with ID 567.
var objID = new MFaaP.MFWSClient.ObjID()
{
	Type = 0,
	ID = 567
};
client.ObjectOperations.DeleteObject(objID);
```

### Undeleting

```csharp
// Instantiate a new MFWS client.
var client = new MFWSClient("http://m-files.mycompany.com");

// Authentiate to a vault with GUID {C840BE1A-5B47-4AC0-8EF7-835C166C8E24} (clear credentials).
client.AuthenticateUsingCredentials(Guid.Parse("{C840BE1A-5B47-4AC0-8EF7-835C166C8E24}"), "MyUsername", "MyPassword")

// Delete the document (type ID 0) with ID 567.
var objID = new MFaaP.MFWSClient.ObjID()
{
	Type = 0,
	ID = 567
};
client.ObjectOperations.UndeleteObject(objID);
```