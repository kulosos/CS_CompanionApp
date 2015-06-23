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
	resetInputError();
	
	if(validateIPaddress(connection.ip) && validatePort(connection.port)){
		engine.trigger("getConnectionParameter", connection.ip + ":" + connection.port);
		switchGameUI();
		if(D)console.log("Connection: " + connection.ip + ":" + connection.port);
	}
}

function validateIPaddress(ipaddress) {  
	if (/^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/.test(ipaddress))  {  
    	return (true);
  	}
	setInputError("Invalid IP-Adress");
	return (false);
}  

function validatePort(port){
	
	if(/^\d+$/.test(port)){
		console.info("valid port");
		return (true);
	}
	setInputError("Invalid Port");
	return(false);
}

function setInputError(msg){
	$("#content").find(".inputError").css("visibility", "visible");
	$("#content").find(".inputError").text(msg);
}

function resetInputError(){
	$("#content").find(".inputError").css("visibility", "hidden");
	$("#content").find(".inputError").text("");
}