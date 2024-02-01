import * as leaflet from 'leaflet';
//43°19'28.99"N, 21°54'11.99"E
const nisLocation = {
  coords: leaflet.latLng(43.32, 21.9),
  zoom: 15,
};

export class LeafletMap {
  public map: leaflet.Map;
  

  constructor(id: string, markerY: number, markerX: number, iconLabel: string) {

    this.map = leaflet.map(id);

    leaflet
      .tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png')
      .addTo(this.map);

    this.map.setView(nisLocation.coords, nisLocation.zoom);

    var myIconReplc = leaflet.Icon.extend({
      options: {
          iconUrl: "/assets/slike/marker-icon.png",
          iconSize: [15,20],
          iconAnchor: [8, 2] // horizontal puis vertical
      }
  });

    leaflet
      .marker([markerY, markerX]).setIcon(new myIconReplc)
      .addTo(this.map)
      .bindPopup(iconLabel)
      .openPopup();
  }
}
