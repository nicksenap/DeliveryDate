## DeliveryDate Functon

Serverless function/api on **.NET core** and **AWS serverless computing** Lambda, API Gateway。 Also includes cloudformation, SAM, Swagger, xUnit,

### Try out the function:

Try the API through cUrl:

```lang=bash
curl --location --request POST 'https://da9ipj6ehg.execute-api.eu-north-1.amazonaws.com/Prod/' \
--header 'Content-Type: application/json' \
--data-raw '{
  "Products": [
    {
      "ProductId": 1,
      "Name": "Ägg Frigående Inomhus 15-p S 762g",
      "DeliveryDays": [1, 3, 4, 5],
      "ProductType": 1,
      "DaysInAdvance": 1
    },
    {
      "ProductId": 2,
      "Name": "Grädd Ädelost 36%",
      "DeliveryDays": [1, 2, 3, 5],
      "ProductType": 1,
      "DaysInAdvance": 1
    },
    {
      "ProductId": 3,
      "Name": "Köttfria Köttbullar Frysta",
      "DeliveryDays": [1, 3, 4, 5],
      "ProductType": 2,
      "DaysInAdvance": 1
    }
  ],
  "PostalNumber": "13760"
}'
```

then we should get something like this:

```lang=json
[
    {
        "postalCode": "13760",
        "deliveryDate": "2021-12-08T00:00:00Z",
        "isGreenDelivery": true
    },
    {
        "postalCode": "13760",
        "deliveryDate": "2021-12-06T00:00:00Z",
        "isGreenDelivery": false
    },
    {
        "postalCode": "13760",
        "deliveryDate": "2021-12-10T00:00:00Z",
        "isGreenDelivery": false
    },
    {
        "postalCode": "13760",
        "deliveryDate": "2021-12-13T00:00:00Z",
        "isGreenDelivery": false
    }
]
```

### Test:

- cd to project `/test/DeliveryDate.SAM.Tests`
- Run the following command
  > dotnet test

### Deployment:

Running the **dotnet cli** to deploy serverless stack, it will deploy API Gateway, Lambda.

- Install **dotnet CLI**

- Install **dotnet Lambda tools**

  > dotnet tool install -g Amazon.Lambda.Tools

- cd to project `/src/DeliveryDate.SAM`

- Run the following command
  > dotnet lambda deploy-serverless

### Rollback:

- Open _AWS Console_

- Navigate to _cloudformation_

- Delete the created stack
