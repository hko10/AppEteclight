using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Plugin.Battery;
using Xamarin.Essentials;

namespace AppEtecLight
{
    public partial class MainPage : ContentPage
    {
        bool lanterna_ligada = false;

        public MainPage()
        {
            InitializeComponent();

            BtnOnOff.Source = ImageSource.FromResource("AppEtecLight.Image.switch-off.png");

            Carrega_Informacoes_Bateria();
        }

        private async void Carrega_Informacoes_Bateria()
        {
            try
            {
                if (CrossBattery.IsSupported)
                {
                    CrossBattery.Current.BatteryChanged -= Mudanca_Status_Bateria;
                    CrossBattery.Current.BatteryChanged += Mudanca_Status_Bateria;
                }
                else
                    lbl_bateria_fraca.Text = "As informações sobre a bateria não estão disponíveis.";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ocorreu um erro", ex.Message, "OK");
            }
        }

        private async void Mudanca_Status_Bateria(object sender, Plugin.Battery.Abstractions.BatteryChangedEventArgs e)
        {
            try
            {
                lbl_porcentagem_restante.Text = e.RemainingChargePercent.ToString() + "%";

                if (e.IsLow)
                    lbl_bateria_fraca.Text = "Atenção! A bateria está fraca!";
                else
                    lbl_bateria_fraca.Text = "";

                switch (e.Status)
                {
                    case Plugin.Battery.Abstractions.BatteryStatus.Charging:
                        lbl_status.Text = "Carregando";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Discharging:
                        lbl_status.Text = "Descarregando";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Full:
                        lbl_status.Text = "Carregado";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.NotCharging:
                        lbl_status.Text = "Sem carregar";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Unknown:
                        lbl_status.Text = "Desconhecido";
                        break;
                }

                switch (e.PowerSource)
                {
                    case Plugin.Battery.Abstractions.PowerSource.Ac:
                        lbl_fonte_carregamento.Text = "Carregador";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Battery:
                        lbl_fonte_carregamento.Text = "Bateria";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Usb:
                        lbl_fonte_carregamento.Text = "USB";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Wireless:
                        lbl_fonte_carregamento.Text = "Sem fio";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Other:
                        lbl_fonte_carregamento.Text = "Desconhecido";
                        break;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ocorreu um erro", ex.Message, "OK");
            }
        }

        private async void BtnOnOff_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!lanterna_ligada)
                {
                    lanterna_ligada = true;
                    BtnOnOff.Source = ImageSource.FromResource("AppEtecLight.Image.switch-on.png");

                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));

                    await Flashlight.TurnOnAsync();
                }
                else
                {
                    lanterna_ligada = false;
                    BtnOnOff.Source = ImageSource.FromResource("AppEtecLight.Image.switch-off.png");

                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));

                    await Flashlight.TurnOffAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ocorreu um erro", ex.Message, "OK");
            }
        }
    }
}
