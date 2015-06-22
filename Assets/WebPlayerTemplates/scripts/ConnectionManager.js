/*
 * Connection Script
 *
 * @brief: Controls connect/disconnet to network game
 * 
 * @autor: Oliver Kulas
 * @version: 1.0
 * @date: Jun 2015
 *
 */
//-----------------------------------------------------------------------------

var connectBtn;
var connection = { ip: "127.0.0.1", port: "25000" } 

function initConnectionMananger() {
	
	
	// console.log("init ConnectionManager");
	
}

function connect(){
	
	connection.ip = $("#content").find('#ipAdress').val(), 
	connection.port = $("#content").find('#port').val();
	
	engine.trigger("getConnectionParameter", connection.ip + ":" + connection.port);
		
	
	if(D)console.log("Connection: " + connection.ip + ":" + connection.port);
	
}