using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using static GMap.NET.Entity.OpenStreetMapGraphHopperGeocodeEntity;

namespace Task1
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private GMap.NET.WindowsForms.GMapMarker currentMarker = null;//current marker
        bool markerSlected = false;//if marker selected
        DataBase dataBase = new DataBase();//DataBase object

        private void loadMap(object sender, EventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance; //Google map provider 
            gMapControl1.MinZoom = 2; 
            gMapControl1.MaxZoom = 16; 
            gMapControl1.Zoom = 4; 
            gMapControl1.Position = new GMap.NET.PointLatLng(54.967695, 82.892850);//Novosibirsk point
            gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter; 
            gMapControl1.CanDragMap = true; 
            gMapControl1.DragButton = MouseButtons.Left; 
            gMapControl1.ShowCenter = false; 
            gMapControl1.ShowTileGridLines = false;

            setUpMarkersFromDB();
        }
       
        private void markerEntered(GMap.NET.WindowsForms.GMapMarker item)//Entering marker
        {
            currentMarker = item;
        }
       
        private void markerLeaved(GMap.NET.WindowsForms.GMapMarker item)//Leaving marker
        {
            currentMarker = null;
        }
       
        private void mouseMoved(object sender, MouseEventArgs e)//Moving mouse around the map
        {
            if (currentMarker != null && markerSlected)//If grapped marker
            {
                // Selected point
                PointLatLng point = gMapControl1.FromLocalToLatLng(e.X, e.Y);
                // Marker coordinates
                currentMarker.Position = point;
            }
        }
        
        private void moseUp(object sender, MouseEventArgs e)//Leaving mouse button
        {
            if (currentMarker != null)
            {
                int id = Convert.ToInt32(currentMarker.Tag);
                double lat = currentMarker.Position.Lat;
                double lng = currentMarker.Position.Lng;
                dataBase.createSetQuery(id, lng, lat);//Updating DB
                currentMarker = null; 
            }
            markerSlected = false;
        }
        
        private void mouseDown(object sender, MouseEventArgs e)//Pressing mouse button
        {
            if (currentMarker != null)
            {
                markerSlected = true;
            }
        }
       
        private void setUpMarkersFromDB ()//Getting info about markers from DB and placing them on the map
        {
            dataBase.openConnection();
            List<Rigs> listRigs = dataBase.createGetQuery(@"SELECT * FROM Rigs");//Returning the list of all markers in DB
            GMap.NET.WindowsForms.GMapOverlay markers = new GMap.NET.WindowsForms.GMapOverlay("markers");//Creating a layer of markers

            if (listRigs != null)
            {
                foreach (Rigs rig in listRigs)
                {
                    GMap.NET.WindowsForms.GMapMarker marker = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                        new GMap.NET.PointLatLng(rig.lat, rig.lng),
                        GMap.NET.WindowsForms.Markers.GMarkerGoogleType.blue_pushpin);
                    marker.ToolTipText = rig.name;
                    marker.Tag = rig.id;//Setting Tag which would be ID of the marker
                    markers.Markers.Add(marker);
                }

                gMapControl1.Overlays.Add(markers);
            }
        
            dataBase.closeConnection();
        }

        private void formClosed(object sender, FormClosedEventArgs e)
        {
            dataBase.closeConnection();
        }//Closing form
    }

}
