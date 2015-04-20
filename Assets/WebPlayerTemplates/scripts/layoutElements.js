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


$(document).ready(function(){
	//updateElementsWidth();
});

function updateElementsWidth(){

	// outer wrapper
	itemSize = Math.floor((globals.docWidth * 0.8) / 4);
	outerWrapperWidth = itemSize * 4 + (8*5); //(8*5) equals the margins
	$("#outerWrapper").css("left", Math.floor(globals.docWidth/2));
	$("#outerWrapper").css("width", outerWrapperWidth);
	$("#outerWrapper").css("margin-left", (outerWrapperWidth * -1)/2);

	// header
	$(".mainMenuHeader").css("width", itemSize * 4 + (3*5));
	$(".mainMenuHeader").css("height", Math.floor(itemSize / 3.5));

	// footer
	$(".mainMenuFooter").css("width", itemSize * 4 + (3*5));
	$(".mainMenuFooter").css("height",  Math.floor(itemSize / 3.5));

	// menu items
	$(".mainMenuBtn").css("width", itemSize);
	$(".mainMenuBtn").css("height", itemSize);

	//console.log("docWidth: ", docWidth);
	//console.log("outerWrapperWidth: ", outerWrapperWidth);
	//console.log("itemSize: ", itemSize);
};

$(window).resize(function() { updateElementsWidth(); });
