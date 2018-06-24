
/**  
            * 天地图地图类型说明  
            ______________________________  
                图层名称  |  说明  
               vec_c  | 矢量  
               img_c  | 影像  
               ter_c  | 地形  
               cva_c  | 注记  
               eva_c  | 英文注记  
               cia_c  | 路网  
               eia_c  | 英文路网  
            ————————————————————————  
            */
mapCls = {
    map: null,
  //  kmlLayers:[],
    initMap: function (mapId) {
        var matrixIds = [];
        for (var i = 0; i < 22; ++i) {
            matrixIds[i] = {
                identifier: 1 + i,
                topLeftCorner: new L.LatLng(90, -180)
            };
        }

        var emap_url = 'http://t4.tianditu.com/vec_c/wmts';
        var emapanno_url = 'http://t4.tianditu.com/cva_c/wmts';
        var img_url = 'http://t4.tianditu.com/img_c/wmts';
        var imganno_url = 'http://t4.tianditu.com/cia_c/wmts';
        ////电子地图
        //var emap_layer = new L.TileLayer.WMTS(emap_url,
        //                           {
        //                               tileSize: 256,
        //                               layer: 'vec',
        //                               style: "default",
        //                               tilematrixSet: "c",
        //                               format: "tile"
        //                           }
        //                          );
        ////电子地图注记
        //var emapanno_layer = new L.TileLayer.WMTS(emapanno_url,
        //                           {
        //                               tileSize: 256,
        //                               layer: 'cva',
        //                               style: "default",
        //                               tilematrixSet: "c",
        //                               format: "tile"
        //                           }
        //                          );
        //影像地图						  
        var img_layer = new L.TileLayer.WMTS(img_url,
                                   {
                                       tileSize: 256,
                                       layer: 'img',
                                       style: "default",
                                       tilematrixSet: "c",
                                       format: "tile",
                                       matrixIds: matrixIds

                                   }
                                  );
        //影像地图注记						  
        var imganno_layer = new L.TileLayer.WMTS(imganno_url,
                                   {
                                       tileSize: 256,
                                       layer: 'cia',
                                       style: "default",
                                       tilematrixSet: "c",
                                       format: "tile",
                                       matrixIds: matrixIds
                                       
                                   }
                                  );
        var ts = this;
        var mid = mapId ? mapId : 'map';
        ts.map = L.map(mid, {
            crs: L.CRS.EPSG4326,
            attributionControl: false,
            renderer: L.canvas({padding:0.5}),
            trackResize: false,
            center: {
                lon: 114.31, //经度
                lat: 30.59 // 纬度
            },
            minZoom:4,
            maxZoom:16,
            zoom: 7

        });
        ts.map.addLayer(img_layer);
        ts.map.addLayer(imganno_layer);
        L.control.scale({position:'bottomright',imperial:false}).addTo(ts.map);
    },
    clearMap: function () {
        this.map.remove();
        this.kmlLayers = [];
    },
    removeAllKmls: function () {
        var me = this;
        if(!me.map) return;
        for (var i = 0; i < me.kmlLayers.length; i++) {
            ts.map.removeLayer(me.kmlLayers[i]);
        }
    },

    addKmls: function (lstKmlUrl) {
        var me = this;
       // me.kmlLayer = [];
        var obj = {};
        //add kmls
        for (var i = 0; i < lstKmlUrl.length; i++) {
            var dt = lstKmlUrl[i];
            var layer = new L.KML(dt.url, { async: true });
            layer.on("loaded", function (e) {
                me.map.fitBounds(e.target.getBounds());
            });
            obj[dt.name] = layer;
            me.map.addLayer(layer);
        }
        var opt = { collapsed: false, hideSingleBase: true };
        me.map.addControl(new L.Control.Layers({}, obj, opt));

    },
    //showKmls: function (lstIndx) {
    //    //remove all kml layer
    //    for (var i = 0; i < kmlLayers.length; i++) {
    //        if (map.hasLayer(kmlLayers[i])){
    //            map.removeLayer(kmlLayers[i]);
    //        }
    //    }

    //    //add show layer
    //    for (var i = 0; i < lstIndx.length; i++) {
    //        map.addLayer(kmlLayers[i]);
    //    }
    //    map.addLayer(layer);
    //},
    setView: function (lat,lon,zoom) {
        if (this.map) {
            this.map.setView(L.latLng(lat, lon), zoom);
        }
    },

    addMarker: function () {

    },
    addMarkers: function () {

    },
    //obj: {"popMsg":"poly1","popCoor":[31.3579,112.86321], "color":"#F00","coordinates":[[31.35914,112.86394],[31.3579,112.86321]]}
    addPolygon: function (obj) {
        if (!obj) return;
        var co = obj.color ? obj.color : "#f00";
        var polygon = L.polygon(obj.coordinates, { color: co}).addTo(this.map);
        if (obj.popMsg) {
            var coor = obj.popCoor ? obj.popCoor : obj.coordinates[0];
            var popup = L.popup()
               .setLatLng(coor)
               .setContent(obj.popMsg);       //'<p><h3>Hello world!</h3><br />This is a nice popup.</p>')
            polygon.bindPopup(popup);
        }
        polygon.on('mousemove', function (e) {
            polygon.setStyle({ color: "#ff7800", weight: 1 });
        });
        polygon.on('mouseout', function (e) {
            polygon.setStyle({ color: "#f00", weight: 1 });
        });
    },
    addPolygons: function (objs) {
        var vt = this;
        var len = objs? objs.length:0;
        for (var i = 0; i < len; i++) {
            vt.addPolygon(objs[i]);
        }
        
    }
    
}


