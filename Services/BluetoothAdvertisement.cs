
//using Shiny.BluetoothLE;
//using Shiny.BluetoothLE.Hosting;

//namespace MauiApp1.Services;

//internal class BluetoothAdvertisement
//{
//    private readonly IPeripheralManager peripheralManager;

//    public BluetoothAdvertisement()
//    {
//        this.peripheralManager = ShinyHost.Resolve<IPeripheralManager>();
//    }

//    public async Task StartAdvertisement()
//    {
//        var serviceUuid = new Guid("12345678-1234-1234-1234-1234567890ab");
//        var service = this.peripheralManager.CreateService(serviceUuid);

//        var characteristicUuid = new Guid("YOUR-CHARACTERISTIC-UUID");
//        service.AddCharacteristic(characteristicUuid, characteristicBuilder =>
//        {
//            characteristicBuilder.SetProperties(CharacteristicsPropertyType.Read);
//            characteristicBuilder.SetPermissions(GattPermissions.Read);
//            characteristicBuilder.OnReadRequest(async request =>
//            {
//                var nameBytes = Encoding.UTF8.GetBytes("Your Service Name");
//                await request.RespondWithValue(nameBytes);
//            });
//        });

//        await this.peripheralManager.AddService(service);
//        await this.peripheralManager.StartAdvertising(new AdvertisementOptions
//        {
//            LocalName = "Your Device Name",
//            ServiceUuids = new List<Guid> { serviceUuid }
//        });
//    }
//}
//;


//    await peripheralManager.AddService(service);
//    await peripheralManager.StartAdvertising(new AdvertisementOptions
//    {
//        LocalName = "Your Device Name",
//        ServiceUuids = new List<Guid> { serviceUuid }
//    });


//}
