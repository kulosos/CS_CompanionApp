/*
 * Blur Elements Script
 *
 * @brief: blur elements backgrounds, realized with CSS3.0 and JS
 * 
 * @autor: Oliver Kulas
 * @version: 1.0
 * @date: Jan 2015
 *
 */
  
//-----------------------------------------------------------------------------

var blurRadius = 50;

function blurElement(element, source){

	console.log("blur class");

	$('.start_contentBox').blurjs({
		source: '.outerWrapper',
		radius: blurRadius,
		overlay: 'rgba(255,255,255,0.5)'
	});
}

/*
$(document).ready(function(){
			blurHeader();
});
		
function blurHeader(){
	var content = document.querySelector('.content');
	var duplicate = content.cloneNode(true);
	var contentBlurred = document.createElement('div');
	contentBlurred.className = 'content-blurred';
	contentBlurred.appendChild(duplicate);
	
	var header = document.querySelector('header');
	header.appendChild(contentBlurred);
	
	var contentWrapper = document.querySelector('.content-wrapper'),
	translation;
	
	contentWrapper.addEventListener('scroll',function(){
	  translation = 'translate3d(0,' + (-this.scrollTop + 'px') + ',0)';
	  duplicate.style['-webkit-transform'] = translation;
	  duplicate.style['-moz-transform'] = translation;
	  duplicate.style['transform'] = translation;
	  
	  //console.log(duplicate);
	});
};

function blurHeaderStatic(){
	$('.header').blurjs({
		source: 'body',
		radius: 50,
		overlay: 'rgba(255,255,255,0.5)'
	});
};


function blurContentBox(){
	$('.contentBox').blurjs({
		source: 'body',
		radius: 50,
		overlay: 'rgba(255,255,255,0.5)'
	});
};

// FIXME
// Is this still used? I don't think so.
/*$(window).resize(function() { 
	blurContentBox(); 
	blurHeaderStatic();
});







function updateBlurBG(){
	$('#blurryHeader').blurjs({
		source: '#header',					//Background to blur
		radius: 20,							//Blur Radius
		overlay: 'rgba(255,255,255,0.4)',	//Overlay Color, follow CSS3's rgba() syntax
		offset: {							//Pixel offset of background-position
			x: 0,
			y: 0
		},
		optClass: '',						//Class to add to all affected elements
		cache: false,						//If set to true, blurred image will be cached and used in the future. If image is in cache already, it will be used.
		cacheKeyPrefix: 'blurjs-',			//Prefix to the keyname in the localStorage object
		draggable: false						//Only used if jQuery UI is present. Will change background-position to fixed
	});
};

function blurHeader(){
	$('#header').blurjs({
		source: 'body',
		overlay: 'rgba(255,255,255,0.4)',
		
	});	
}*/

