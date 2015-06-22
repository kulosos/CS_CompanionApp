/*
 * Construction Simulator 2015 Companion App
 *
 * @brief: Sets layout element attributes
 * 
 * @autor: Oliver Kulas
 * @version: 1.0
 * @date: April 2015
 */
  
//-----------------------------------------------------------------------------

var outerWrapperWidth;
var itemSize;

function initLayoutController(){
	updateElementsWidth();
}

function updateElementsWidth(){

	// header
	var headerRightWidth = $(window).width() - parseInt($(".headerLeft").css("width"));
	$(".headerRight").css("width", headerRightWidth);
	//if(D)console.info("Updating UI Elements");

};

$(window).resize(function() { updateElementsWidth(); });
