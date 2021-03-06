﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IP_SharedLibrary.Entity;
using IP_SharedLibrary.Packet;
using System.Windows.Forms.DataVisualization.Charting;
using IP_SharedLibrary.Packet.Push;

namespace RHAPP_IP_Client
{
    public partial class DoctorForm : StandardForm
    {
        public List<DataPoint> speedPoints { get; set; } = new List<DataPoint>();
        public List<DataPoint> bpmPoints { get; set; } = new List<DataPoint>();
        public List<DataPoint> rpmPoints { set; get; } = new List<DataPoint>();

        public DoctorForm()
        {
            InitializeComponent();
            _appGlobal.UserChangedEvent += HandleUserChanged;
            _appGlobal.IncomingMeasurementEvent += HandleIncomingMeasurement;
            cmbOnlinePatients.ValueMember = null;
            cmbOnlinePatients.DisplayMember = "Nickname";
        }

        private void HandleUserChanged(User u)
        {
            if (u.Username == _appGlobal.Username)
                return;
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleUserChanged(u)));
                return;
            }
            RemoveUsersFromcmbBox();
            LoadUsers(_appGlobal.Users.Where(x => x.Username != _appGlobal.Username).ToList());
        }

        private void HandleIncomingMeasurement(Packet packet)
        {
            var selectedUser = (User)cmbOnlinePatients.SelectedItem;
            var resultPacket = ((SerialDataPushPacket)packet);
            if (selectedUser.Username == resultPacket.Username)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => HandleIncomingMeasurement(packet)));
                }
                else
                {
                    HandleBikeData(resultPacket.Measurement);
                }
            }
        }


        private void HandleBikeData(Measurement m)
        {
            //fill graph pulse
            bpmPoints.Add(new DataPoint(m.Time.ToOADate(), Convert.ToDouble(m.Pulse)));
            crtPulse.Series[0].Points.Clear();
            for (int i = 0; i < bpmPoints.Count; i++)
                crtPulse.Series[0].Points.Add(bpmPoints[i]);
            if (bpmPoints.Count > 25)
                bpmPoints.RemoveAt(0);
            crtPulse.Update();

            //fill graph rpm
            rpmPoints.Add(new DataPoint(m.Time.ToOADate(), Convert.ToDouble(m.PedalRpm)));
            crtRPM.Series[0].Points.Clear();
            for (int i = 0; i < rpmPoints.Count; i++)
                crtRPM.Series[0].Points.Add(rpmPoints[i]);
            if (rpmPoints.Count > 25)
                rpmPoints.RemoveAt(0);
            crtRPM.Update();
        }

        public void LoadUsers(List<User> nicknames)
        {
            foreach (User u in nicknames)
            {
                cmbOnlinePatients.Items.Add(u);
            }
        }

        public void RemoveUsersFromcmbBox()
        {
            cmbOnlinePatients.Items.Clear();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        int j = 0;
        private void btnTestButton_Click(object sender, EventArgs e)
        {
            int i;
            for (i = j; i < (j + 3); i++)
            {
                _appGlobal.Users.Add(new User("nickname" + i.ToString(), "username" + i.ToString(), null));
                HandleUserChanged(_appGlobal.Users.Last());
            }
            j = i;

        }

        private void DoctorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.FormClosingMethod(sender, e, this);
        }

        private void btnSetPower_Click(object sender, EventArgs e)
        {

        }

        private void btnStartTest_Click(object sender, EventArgs e)
        {

        }

        private void cmbOnlinePatients_SelectionChangeCommitted(object sender, EventArgs e)
        {
            speedPoints.Clear();
            bpmPoints.Clear();
            rpmPoints.Clear();
            List<Measurement> measurementsOfPatient = _appGlobal.PatientMeasurements
                .Where(patient => patient.Key == ((User)cmbOnlinePatients.SelectedItem).Username)
                .Select(allMeasurements => allMeasurements.Value).ToList();
            foreach(Measurement measurement in measurementsOfPatient)
            {
                HandleBikeData(measurement);
            }
            var lastMeasurement = measurementsOfPatient.LastOrDefault();
            if (lastMeasurement != null)
                crtPower.Value = lastMeasurement.DestPower;

        }
    }
}
