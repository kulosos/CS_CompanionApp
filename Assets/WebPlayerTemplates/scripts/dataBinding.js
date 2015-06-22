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

var globals = {
	//docHeightJQ: 	$(document).height(),
	//docWidthJQ: 	$(document).width(),
	docHeight: 		$(window).height(),
	docWidth: 		$(window).width()
}


$(document).ready(function(){
	//bindData();
});

function bindData(){
	engine.on("dataBindingEvent", triggeredEventFromUnity);
	engine.on("Ready");
}

function triggeredEventFromUnity(receivedArguments){

	$(".wrapperHeader").append(receivedArguments);
	console.log("hier: " + receivedArguments);

	//mainMenuUITest();

}
/*
function test4(){
	//console.log("test4 dingens");
	engine.trigger("functionName", "kram von drinnen in dem string");

	engine.trigger("functionName", "irgendeinen string");
	console.log(engine.call("functionName", "bla anderer string"));
}*/


function mainMenuUITest(){
	engine.trigger("functionName", "kram von drinnen in dem string");

	engine.trigger("functionName", "irgendeinen string");
	console.log(engine.call("functionName", "bla anderer string"));
}

function debugInfosToUnity(){

	//var str = "JQuery (h/w): " + globals.docHeightJQ + ", " + globals.docWidthJQ	+ "\nJavaScript (h/w): " + globals.docHeightJS + ", " + globals.docWidthJS;
	var str = "JavaScript (h/w): " + globals.docHeight + ", " + globals.docWidth;

	engine.trigger("debugInfo", str);
	//engine.call("functionName");
	console.log(str);

}
