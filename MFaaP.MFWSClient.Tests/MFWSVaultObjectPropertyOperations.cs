﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace MFaaP.MFWSClient.Tests
{
	[TestClass]
	public class MFWSVaultObjectPropertyOperations
	{

		#region Path encoding for unmanaged objects

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.GetPropertiesAsync(ObjVer,System.Threading.CancellationToken)"/>
		/// correctly encodes paths for unmanaged objects.
		/// </summary>
		[TestMethod]
		public async Task GetPropertiesAsync_ExternalObject_LatestVersion()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<PropertyValue>>(Method.Get, $"/REST/objects/0/umyrepository%3A12%2B3456/latest/properties.aspx");

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.GetPropertiesAsync(new ObjVer()
			{
				Type = 0,
				VersionType = MFObjVerVersionType.Latest,
				ExternalRepositoryName = "myrepository",
				ExternalRepositoryObjectID = "12 3456" // NOTE: This will be double-encoded (" " to "+", then to "%2B").
			});

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.GetPropertiesAsync(ObjVer,System.Threading.CancellationToken)"/>
		/// correctly encodes paths for unmanaged objects.
		/// </summary>
		[TestMethod]
		public async Task GetPropertiesAsync_ExternalObject_SpecificVersion()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<PropertyValue>>(Method.Get, $"/REST/objects/0/umyrepository%3A12%2B3456/uabc%253A123/properties.aspx");

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.GetPropertiesAsync(new ObjVer()
			{
				Type = 0,
				VersionType = MFObjVerVersionType.Latest,
				ExternalRepositoryName = "myrepository",
				ExternalRepositoryObjectID = "12 3456", // NOTE: This will be double-encoded (" " to "+", then to "%2B").
				ExternalRepositoryObjectVersionID = "abc:123" // NOTE: This will be double-encoded (":" to "%3A", then to "%253A").
			});

			// Verify.
			runner.Verify();
		}

		#endregion

		#region GetProperties (single object)

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.GetProperties(MFaaP.MFWSClient.ObjVer,System.Threading.CancellationToken)"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void GetProperties_ObjVer()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<PropertyValue>>(Method.Get, "/REST/objects/1/2/4/properties.aspx");

			// Execute.
			runner.MFWSClient.ObjectPropertyOperations.GetProperties(1, 2, 4);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.GetPropertiesAsync(MFaaP.MFWSClient.ObjVer,System.Threading.CancellationToken)"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task GetPropertiesAsync_ObjVer()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<PropertyValue>>(Method.Get, "/REST/objects/1/2/4/properties.aspx");

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.GetPropertiesAsync(1, 2, 4);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.GetProperties(ObjID,System.Threading.CancellationToken)"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void GetProperties_ObjID()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<PropertyValue>>(Method.Get, "/REST/objects/1/2/latest/properties.aspx");

			// Execute.
			runner.MFWSClient.ObjectPropertyOperations.GetProperties(1, 2);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.GetPropertiesAsync(ObjID,System.Threading.CancellationToken)"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task GetPropertiesAsync_ObjID()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<PropertyValue>>(Method.Get, "/REST/objects/1/2/latest/properties.aspx");

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.GetPropertiesAsync(1, 2);

			// Verify.
			runner.Verify();
		}

		#endregion

		#region GetProperties (multiple object versions)

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.GetPropertiesOfMultipleObjects(MFaaP.MFWSClient.ObjVer[])"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void GetPropertiesOfMultipleObjects()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<List<PropertyValue>>>(Method.Post, "/REST/objects/properties.aspx");

			// Create the object to send in the body.
			var body = new ObjVer()
			{
				ID = 2,
				Type = 1,
				Version = 4
			};

			// We should post a collection of objvers (but only with this one in it).
			runner.SetExpectedRequestBody(new[] { body });

			// Execute.
			runner.MFWSClient.ObjectPropertyOperations.GetPropertiesOfMultipleObjects(body);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.GetPropertiesOfMultipleObjectsAsync(MFaaP.MFWSClient.ObjVer[])"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task GetPropertiesOfMultipleObjectsAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<List<PropertyValue>>>(Method.Post, "/REST/objects/properties.aspx");

			// Create the object to send in the body.
			var body = new ObjVer()
			{
				ID = 2,
				Type = 1,
				Version = 4
			};

			// We should post a collection of objvers (but only with this one in it).
			runner.SetExpectedRequestBody(new[] { body });

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.GetPropertiesOfMultipleObjectsAsync(body);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.GetPropertiesOfMultipleObjects(MFaaP.MFWSClient.ObjVer[])"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		[ExpectedException(typeof(System.ArgumentException))]
		public void GetPropertiesOfMultipleObjects_ExceptionForNoVersion()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<List<PropertyValue>>>(Method.Post, "/REST/objects/properties.aspx");

			// Create the object to send in the body.
			var body = new ObjVer()
			{
				ID = 2,
				Type = 1
			};

			// We should post a collection of objvers (but only with this one in it).
			runner.SetExpectedRequestBody(new[] { body });

			// Execute.
			runner.MFWSClient.ObjectPropertyOperations.GetPropertiesOfMultipleObjects(body);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.GetPropertiesOfMultipleObjectsAsync(MFaaP.MFWSClient.ObjVer[])"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		[ExpectedException(typeof(System.ArgumentException))]
		public async Task GetPropertiesOfMultipleObjectsAsync_ExceptionForNoVersion()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<List<PropertyValue>>>(Method.Post, "/REST/objects/properties.aspx");

			// Create the object to send in the body.
			var body = new ObjVer()
			{
				ID = 2,
				Type = 1
			};

			// We should post a collection of objvers (but only with this one in it).
			runner.SetExpectedRequestBody(new[] { body });

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.GetPropertiesOfMultipleObjectsAsync(body);

			// Verify.
			runner.Verify();
		}

		#endregion

		#region SetProperty

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.SetProperty(int,int,MFaaP.MFWSClient.PropertyValue,System.Nullable{int},System.Threading.CancellationToken)"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void SetProperty()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ExtendedObjectVersion>(Method.Put, "/REST/objects/1/2/latest/properties/0");

			// Create the object to send in the body.
			var body = new PropertyValue()
			{
				PropertyDef = 0,
				TypedValue = new TypedValue()
				{
					DataType = MFDataType.Text,
					Value = "hello world"
				}
			};
			runner.SetExpectedRequestBody(body);

			// Execute.
			runner.MFWSClient.ObjectPropertyOperations.SetProperty(1, 2, body);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.SetPropertyAsync(int,int,MFaaP.MFWSClient.PropertyValue,System.Nullable{int},System.Threading.CancellationToken)"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task SetPropertyAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ExtendedObjectVersion>(Method.Put, "/REST/objects/1/2/latest/properties/0");

			// Create the object to send in the body.
			var body = new PropertyValue()
			{
				PropertyDef = 0,
				TypedValue = new TypedValue()
				{
					DataType = MFDataType.Text,
					Value = "hello world"
				}
			};
			runner.SetExpectedRequestBody(body);

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.SetPropertyAsync(1, 2, body);

			// Verify.
			runner.Verify();
		}

		#endregion

		#region RemoveProperty

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.RemoveProperty(int,int,int,System.Nullable{int},System.Threading.CancellationToken)"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void RemoveProperty()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ExtendedObjectVersion>(Method.Delete, "/REST/objects/1/2/latest/properties/0.aspx");

			// Execute.
			runner.MFWSClient.ObjectPropertyOperations.RemoveProperty(1, 2, 0);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.RemovePropertyAsync(int,int,int,System.Nullable{int},System.Threading.CancellationToken)"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task RemovePropertyAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ExtendedObjectVersion>(Method.Delete, "/REST/objects/1/2/latest/properties/0.aspx");

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.RemovePropertyAsync(1, 2, 0);

			// Verify.
			runner.Verify();
		}

		#endregion

		#region SetProperties

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.SetProperties(int,int,MFaaP.MFWSClient.PropertyValue[], bool,System.Nullable{int},System.Threading.CancellationToken)"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void SetProperties_ReplaceAllProperties()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ExtendedObjectVersion>(Method.Put, "/REST/objects/1/2/latest/properties");

			// Create the object to send in the body.
			var body = new[]
			{
				new PropertyValue()
				{
					PropertyDef = 0,
					TypedValue = new TypedValue()
					{
						DataType = MFDataType.Text,
						Value = "hello world"
					}
				}
			};
			runner.SetExpectedRequestBody(body);

			// Execute.
			runner.MFWSClient.ObjectPropertyOperations.SetProperties(1, 2, body, true);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.SetProperties(int,int,MFaaP.MFWSClient.PropertyValue[], bool,System.Nullable{int},System.Threading.CancellationToken)"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void SetProperties_UpdateProperties()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ExtendedObjectVersion>(Method.Post, "/REST/objects/1/2/latest/properties");

			// Create the object to send in the body.
			var body = new[]
			{
				new PropertyValue()
				{
					PropertyDef = 0,
					TypedValue = new TypedValue()
					{
						DataType = MFDataType.Text,
						Value = "hello world"
					}
				}
			};
			runner.SetExpectedRequestBody(body);

			// Execute.
			runner.MFWSClient.ObjectPropertyOperations.SetProperties(1, 2, body, false);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.SetPropertyAsync(int,int,MFaaP.MFWSClient.PropertyValue,System.Nullable{int},System.Threading.CancellationToken)"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task SetPropertiesAsync_ReplaceAllProperties()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ExtendedObjectVersion>(Method.Put, "/REST/objects/1/2/latest/properties");

			// Create the object to send in the body.
			var body = new[]
			{
				new PropertyValue()
				{
					PropertyDef = 0,
					TypedValue = new TypedValue()
					{
						DataType = MFDataType.Text,
						Value = "hello world"
					}
				}
			};
			runner.SetExpectedRequestBody(body);

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.SetPropertiesAsync(1, 2, body, true);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.SetPropertyAsync(int,int,MFaaP.MFWSClient.PropertyValue,System.Nullable{int},System.Threading.CancellationToken)"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task SetPropertiesAsync_UpdateProperties()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ExtendedObjectVersion>(Method.Post, "/REST/objects/1/2/latest/properties");

			// Create the object to send in the body.
			var body = new[]
			{
				new PropertyValue()
				{
					PropertyDef = 0,
					TypedValue = new TypedValue()
					{
						DataType = MFDataType.Text,
						Value = "hello world"
					}
				}
			};
			runner.SetExpectedRequestBody(body);

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.SetPropertiesAsync(1, 2, body, false);

			// Verify.
			runner.Verify();
		}

		#endregion

		#region Set properties of multiple objects at once

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.SetPropertiesOfMultipleObjectsAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public async Task SetPropertiesOfMultipleObjectsAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<ExtendedObjectVersion>>(Method.Put, "/REST/objects/setmultipleobjproperties");

			// Create the expected body.
			var body = new ObjectsUpdateInfo()
			{
				MultipleObjectInfo = new[]
				{
					new ObjectVersionUpdateInformation()
					{
						ObjVer = new ObjVer()
						{
							Type = 0,
							ID = 1,
							Version = 2
						},
						Properties = new List<PropertyValue>
						{
							new PropertyValue()
							{
								PropertyDef = (int) MFBuiltInPropertyDef.MFBuiltInPropertyDefClass,
								TypedValue = new TypedValue()
								{
									Lookup = new Lookup()
									{
										Item = (int) MFBuiltInDocumentClass.MFBuiltInDocumentClassOtherDocument
									}
								}
							}
						}
					},
					new ObjectVersionUpdateInformation()
					{
						ObjVer = new ObjVer()
						{
							Type = 0,
							ID = 2,
							Version = 1
						},
						Properties = new List<PropertyValue>
						{
							new PropertyValue()
							{
								PropertyDef = (int) MFBuiltInPropertyDef.MFBuiltInPropertyDefClass,
								TypedValue = new TypedValue()
								{
									Lookup = new Lookup()
									{
										Item = (int) MFBuiltInDocumentClass.MFBuiltInDocumentClassOtherDocument
									}
								}
							}
						}
					}
				}.ToList()
			};

			// Set the expected body.
			runner.SetExpectedRequestBody(body);

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.SetPropertiesOfMultipleObjectsAsync(
				objectVersionUpdateInformation: body.MultipleObjectInfo.ToArray());

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.SetPropertiesOfMultipleObjects"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		[TestMethod]
		public void SetPropertiesOfMultipleObjects()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<List<ExtendedObjectVersion>>(Method.Put, "/REST/objects/setmultipleobjproperties");

			// Create the expected body.
			var body = new ObjectsUpdateInfo()
			{
				MultipleObjectInfo = new[]
				{
					new ObjectVersionUpdateInformation()
					{
						ObjVer = new ObjVer()
						{
							Type = 0,
							ID = 1,
							Version = 2
						},
						Properties = new List<PropertyValue>
						{
							new PropertyValue()
							{
								PropertyDef = (int) MFBuiltInPropertyDef.MFBuiltInPropertyDefClass,
								TypedValue = new TypedValue()
								{
									Lookup = new Lookup()
									{
										Item = (int) MFBuiltInDocumentClass.MFBuiltInDocumentClassOtherDocument
									}
								}
							}
						}
					},
					new ObjectVersionUpdateInformation()
					{
						ObjVer = new ObjVer()
						{
							Type = 0,
							ID = 2,
							Version = 1
						},
						Properties = new List<PropertyValue>
						{
							new PropertyValue()
							{
								PropertyDef = (int) MFBuiltInPropertyDef.MFBuiltInPropertyDefClass,
								TypedValue = new TypedValue()
								{
									Lookup = new Lookup()
									{
										Item = (int) MFBuiltInDocumentClass.MFBuiltInDocumentClassOtherDocument
									}
								}
							}
						}
					}
				}.ToList()
			};

			// Set the expected body.
			runner.SetExpectedRequestBody(body);

			// Execute.
			runner.MFWSClient.ExternalObjectOperations.PromoteObjects(
				objectVersionUpdateInformation: body.MultipleObjectInfo.ToArray());

			// Verify.
			runner.Verify();
		}

		#endregion

		#region Can the user approve/reject an assignment?

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.CanCompleteAssignmentAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task CanCompleteAssignmentAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<PrimitiveType<bool>>(Method.Get, "/REST/objects/123/latest/canCompleteAssignment.aspx");

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.CanCompleteAssignmentAsync(123);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.CanCompleteAssignment"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void CanCompleteAssignment()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<PrimitiveType<bool>>(Method.Get, "/REST/objects/123/latest/canCompleteAssignment.aspx");

			// Execute.
			runner.MFWSClient.ObjectPropertyOperations.CanCompleteAssignment(123);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.CanCompleteAssignmentAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task CanCompleteAssignmentAsync_ObjID()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<PrimitiveType<bool>>(Method.Get, "/REST/objects/987/latest/canCompleteAssignment.aspx");

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.CanCompleteAssignmentAsync(new ObjID()
			{
				ID = 987,
				Type = (int)MFBuiltInObjectType.MFBuiltInObjectTypeAssignment
			});

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.CanCompleteAssignment"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void CanCompleteAssignment_ObjID()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<PrimitiveType<bool>>(Method.Get, "/REST/objects/987/latest/canCompleteAssignment.aspx");

			// Execute.
			runner.MFWSClient.ObjectPropertyOperations.CanCompleteAssignment(new ObjID()
			{
				ID = 987,
				Type = (int)MFBuiltInObjectType.MFBuiltInObjectTypeAssignment
			});

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.CanCompleteAssignmentAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task CanCompleteAssignmentAsync_ObjID_ThrowsForOtherObjectType()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<PrimitiveType<bool>>(Method.Get, "/REST/objects/987/latest/canCompleteAssignment.aspx");

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.CanCompleteAssignmentAsync(new ObjID()
			{
				ID = 987,
				Type = 101
			});

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.CanCompleteAssignment"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CanCompleteAssignment_ObjID_ThrowsForOtherObjectType()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<PrimitiveType<bool>>(Method.Get, "/REST/objects/987/latest/canCompleteAssignment.aspx");

			// Execute.
			runner.MFWSClient.ObjectPropertyOperations.CanCompleteAssignment(new ObjID()
			{
				ID = 987,
				Type = 101
			});

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.CanCompleteAssignmentAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task CanCompleteAssignmentAsync_ObjVer()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<PrimitiveType<bool>>(Method.Get, "/REST/objects/987/123/canCompleteAssignment.aspx");

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.CanCompleteAssignmentAsync(new ObjVer()
			{
				ID = 987,
				Type = (int)MFBuiltInObjectType.MFBuiltInObjectTypeAssignment,
				Version = 123
			});

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.CanCompleteAssignment"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void CanCompleteAssignment_ObjVer()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<PrimitiveType<bool>>(Method.Get, "/REST/objects/987/123/canCompleteAssignment.aspx");

			// Execute.
			runner.MFWSClient.ObjectPropertyOperations.CanCompleteAssignment(new ObjVer()
			{
				ID = 987,
				Type = (int)MFBuiltInObjectType.MFBuiltInObjectTypeAssignment,
				Version = 123
			});

			// Verify.
			runner.Verify();
		}

		#endregion

		#region Approve assignment

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.ApproveAssignmentAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task ApproveAssignmentAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ObjectVersion>(Method.Put, "/REST/objects/10/123/456/complete.aspx");

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.ApproveAssignmentAsync(new ObjVer()
			{
				ID = 123,
				Type = (int)MFBuiltInObjectType.MFBuiltInObjectTypeAssignment,
				Version = 456
			});

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.ApproveAssignment"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void ApproveAssignment()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ObjectVersion>(Method.Put, "/REST/objects/10/123/456/complete.aspx");

			// Execute.
			runner.MFWSClient.ObjectPropertyOperations.ApproveAssignment(new ObjVer()
			{
				ID = 123,
				Type = (int)MFBuiltInObjectType.MFBuiltInObjectTypeAssignment,
				Version = 456
			});

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.ApproveOrRejectAssignmentAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task ApproveOrRejectAssignmentAsync_Approve()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ObjectVersion>(Method.Put, "/REST/objects/10/123/latest/complete.aspx");

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.ApproveOrRejectAssignmentAsync
			(
				(int)MFBuiltInObjectType.MFBuiltInObjectTypeAssignment,
				123,
				approve: true
			);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.ApproveOrRejectAssignment"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void ApproveOrRejectAssignment_Approve()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ObjectVersion>(Method.Put, "/REST/objects/10/123/latest/complete.aspx");

			// Execute.
			runner.MFWSClient.ObjectPropertyOperations.ApproveOrRejectAssignment
			(
				(int)MFBuiltInObjectType.MFBuiltInObjectTypeAssignment,
				123,
				approve: true
			);

			// Verify.
			runner.Verify();
		}

		#endregion

		#region Reject assignment

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.RejectAssignmentAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task RejectAssignmentAsync()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ObjectVersion>(Method.Put, "/REST/objects/10/123/876/reject.aspx");

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.RejectAssignmentAsync(new ObjVer()
			{
				ID = 123,
				Type = (int)MFBuiltInObjectType.MFBuiltInObjectTypeAssignment,
				Version = 876
			});

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.RejectAssignment"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void RejectAssignment()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ObjectVersion>(Method.Put, "/REST/objects/10/123/876/reject.aspx");

			// Execute.
			runner.MFWSClient.ObjectPropertyOperations.RejectAssignment(new ObjVer()
			{
				ID = 123,
				Type = (int)MFBuiltInObjectType.MFBuiltInObjectTypeAssignment,
				Version = 876
			});

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.ApproveOrRejectAssignmentAsync"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task ApproveOrRejectAssignmentAsync_Reject()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ObjectVersion>(Method.Put, "/REST/objects/10/123/latest/reject.aspx");

			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.ApproveOrRejectAssignmentAsync
			(
				(int)MFBuiltInObjectType.MFBuiltInObjectTypeAssignment,
				123,
				approve: false
			);

			// Verify.
			runner.Verify();
		}

		/// <summary>
		/// Ensures that a call to <see cref="MFaaP.MFWSClient.MFWSVaultObjectPropertyOperations.ApproveOrRejectAssignment"/>
		/// requests the correct resource address with the correct method.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void ApproveOrRejectAssignment_Reject()
		{
			// Create our test runner.
			var runner = new RestApiTestRunner<ObjectVersion>(Method.Put, "/REST/objects/10/123/latest/reject.aspx");

			// Execute.
			runner.MFWSClient.ObjectPropertyOperations.ApproveOrRejectAssignment
			(
				(int)MFBuiltInObjectType.MFBuiltInObjectTypeAssignment,
				123,
				approve: false
			);

			// Verify.
			runner.Verify();
		}

		#endregion

		#region Set workflow states

		[TestMethod]
		public async Task SetWorkflowStateAsync()
		{
			var runner = new RestApiTestRunner<ExtendedObjectVersion>(Method.Put, $"/REST/objects/0/1/2/workflowstate");

			runner.SetExpectedRequestBody(new ObjectWorkflowState() { StateID = 2 });
			// Execute.
			await runner.MFWSClient.ObjectPropertyOperations.SetWorkflowStateAsync(new ObjVer()
			{
				Type = 0,
				ID = 1,
				Version = 2
			}, 2);

			// Verify.
			runner.Verify();
		}

		[TestMethod]
		public void SetWorkflowState()
		{
			var runner = new RestApiTestRunner<ExtendedObjectVersion>(Method.Put, $"/REST/objects/0/1/2/workflowstate");

			runner.SetExpectedRequestBody(new ObjectWorkflowState() { StateID = 2 });
			// Execute.
			runner.MFWSClient.ObjectPropertyOperations.SetWorkflowState(new ObjVer()
			{
				Type = 0,
				ID = 1,
				Version = 2
			}, 2);

			// Verify.
			runner.Verify();
		}

		#endregion
	}
}
