/*
 * Construction Simulator 2015 Companion App
 *
 * @brief: Data binding to Unity for UI over CorrentUI
 * 
 * @autor: Oliver Kulas
 * @version: 1.0
 * @date: April 2015
 */
  
//-----------------------------------------------------------------------------


$(document).ready(function(){
	bindData();
});

function bindData(){
	engine.on("dataBindingEvent", triggeredEventFromUnity);
}

function triggeredEventFromUnity(receivedArguments){

	$(".mainMenuHeader").append(receivedArguments);
	console.log("hier: " + receivedArguments);

	test4();
}

function test4(){
	//console.log("test4 dingens");
	engine.trigger("functionName", "kram von drinnen in dem string");

	engine.trigger("functionName", "irgendeinen string");
	console.log(engine.call("functionName", "bla anderer string"));
}

