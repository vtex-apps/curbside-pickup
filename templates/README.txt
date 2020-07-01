This folder contains Default Mail Templates in JSON format.

{
  "Name": "{{TEMPLATE-NAME-SLUG}}",
  "Description": null,
  "ApplicationId": null,
  "FriendlyName": "{{TEMPLATE-NAME}}",
  "Templates": {
    "email": {
      "Type": "E",
      "CC": null,
      "Message": "{{TEMPLATE-HTML}}",
      "Subject": "{{EMAIL-SUBJECT}}",
      "To": "{{toEmail}}",
      "IsActive": true,
      "ProviderId": "00000000-0000-0000-0000-000000000000"
    },
    "sms": {
      "Type": "S",
      "ProviderId": null,
      "IsActive": false,
      "Parameters": [
        
      ]
    }
  }
}
