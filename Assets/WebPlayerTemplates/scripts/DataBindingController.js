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

function initDataBinding(){
	
	engine.on("Ready", function(){});
	
	engine.on("triggeredEventFromUnity", triggeredEventFromUnity);
	engine.on("switchGameUI", switchGameUI);
	engine.on("setConnectionErrorMsg", setInputError);
}



//-----------------------------------------------------------------------------
/* EXAMPLES
function triggeredEventFromUnity(receivedArguments){
	console.log("Output " + receivedArguments);
}
function jsFunctionName(){
	engine.trigger("functionName", "kram von JS to Unity. Trigger ohne Rückgabeparameter");
	engine.call("functionName", "kram von JS to Unity. Call mit Rückgabeparameter");
}
*/