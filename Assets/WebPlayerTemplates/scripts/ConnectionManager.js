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

var connection = { ip: "127.0.0.1", port: "25000", password: "pw" } ;

function initConnectionMananger() {
	// console.log("init ConnectionManager");	
}

function connect(){
	
	resetInputError();
	setConnectionLoadingBar();
	
	connection.ip = $("#content").find('#ipAdress').val(), 
	connection.port = $("#content").find('#port').val();
	connection.password = $("#content").find("#password").val();
	
	if(validateIPaddress(connection.ip) && validatePort(connection.port)){
		engine.trigger("connect", connection.ip, connection.port, connection.password);
		if(D)console.log("Connection: " + connection.ip + ":" + connection.port + " - " + connection.password);
	}
}

function disconnect(){
	engine.trigger("disconnect");
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

function setConnectionLoadingBar(){
	$("#content").find(".btnConnectText").toggleClass("hide");
	$("#content").find(".connectionLoadingBar").toggleClass("hide");
}