using HomeAero.Interfaces;
using HomeAero.DTO;
using Sensors.Dht;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace HomeAero.Config
{
    public class HomeAeroConfiguration
    {
        // Hardware Pi Variables
        private GpioController _controller;
        private const int ROOT_DHT = 4;
        private const int PLANT_DHT = 5;
        private const int MOTOR_ONE = 12;
        private const int MOTOR_TWO = 13;
        private GpioPin _rootDhtPin;
        private GpioPin _plantDhtPin;
        private GpioPin _motorOnePin;
        private GpioPin _motorTwoPin;
        private IDht _rootDht;
        private IDht _plantDht;

        // Measure Variables
        private double _rootTemp;
        private double _rootHumid;
        private double _plantTemp;
        private double _plantHumid;

        public HomeAeroConfiguration()
        {
            _controller = GpioController.GetDefault();

            if(_controller != null)
            {
                _rootDhtPin = _controller.OpenPin(ROOT_DHT, GpioSharingMode.Exclusive);
                _plantDhtPin = _controller.OpenPin(PLANT_DHT, GpioSharingMode.Exclusive);

                _rootDht = new Dht11(_rootDhtPin, GpioPinDriveMode.Input);
                _plantDht = new Dht11(_plantDhtPin, GpioPinDriveMode.Input);

                _motorOnePin = _controller.OpenPin(MOTOR_ONE);
                _motorOnePin.SetDriveMode(GpioPinDriveMode.Output);
                _motorOnePin.Write(GpioPinValue.High);

                _motorTwoPin = _controller.OpenPin(MOTOR_TWO);
                _motorTwoPin.SetDriveMode(GpioPinDriveMode.Output);
                _motorTwoPin.Write(GpioPinValue.High);
            }

            _rootTemp = 0;
            _rootHumid = 0;
            _plantTemp = 0;
            _plantHumid = 0;
        }

        public async void TakeSensorReading()
        {
            if(_rootDht != null && _plantDht != null)
            {
                try
                {
                    List<Task<(string, bool, double, double)>> sensorReadings = new List<Task<(string, bool, double, double)>>
                    {
                        ReadDht("root", _rootDht),
                        ReadDht("plant", _plantDht)
                    };

                    await Task.WhenAll(sensorReadings);

                    foreach (var reading in sensorReadings)
                    {
                        if(reading.Result.Item2 == true)
                        {
                            if (reading.Result.Item1 == "root")
                            {
                                _rootTemp = reading.Result.Item3;
                                _rootHumid = reading.Result.Item4;
                            }
                            else
                            {
                                _plantTemp = reading.Result.Item3;
                                _plantHumid = reading.Result.Item4;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // TODO: Add error handling - controller didn't initialize
                }
            }
            // Temp until hardware setup - set random values
            else
            {
                var rand = new Random();
                _rootTemp = rand.Next(60, 100);
                _rootHumid = rand.Next(80, 100);
                _plantTemp = rand.Next(60, 80);
                _plantHumid = rand.Next(80, 90);
            }
        }

        public HomeAeroSensorData GetSensorData()
        {
            return new HomeAeroSensorData
            {
                RootTemperature = _rootTemp,
                RootHumidity = _rootHumid,
                PlantTemperature = _plantTemp,
                PlantHumidity = _plantHumid
            };
        }

        public void BeginMisting()
        {
            if(_motorOnePin != null && _motorTwoPin != null)
            {
                _motorOnePin.Write(GpioPinValue.Low);
                _motorTwoPin.Write(GpioPinValue.Low);
            }
        }

        public void EndMisting()
        {
            if(_motorOnePin != null && _motorTwoPin != null)
            {
                _motorOnePin.Write(GpioPinValue.High);
                _motorTwoPin.Write(GpioPinValue.High);
            }
        }

        private async Task<(string identifier, bool valid, double temperature, double humidity)> ReadDht(string identifier, IDht device)
        {
            (string identifier, bool valid, double Temperature, double Humidity) result = (identifier, false, 0, 0);

            DhtReading reading = new DhtReading();
            reading = await device.GetReadingAsync().AsTask();

            if (reading.IsValid)
            {
                var celcius = reading.Temperature;
                var fahrenheit = (celcius * (9.0 / 5.0)) + 32;
                result.Temperature = fahrenheit;
                result.Humidity = reading.Humidity;
                result.valid = true;
            }

            return result;
        }
    }
}
