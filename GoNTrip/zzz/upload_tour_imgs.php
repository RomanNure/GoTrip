<?php
	header("Access-Control-Allow-Origin: *");

	foreach($_FILES as $k => $file) {
		foreach($file as $key => $val) {
			file_put_contents("tour_log.txt", $key." => ".$val."\n", FILE_APPEND);
		}
	}

	$file = $_FILES['tour_img'];	
	file_put_contents("tour_log.txt", "error: ".$file['error']."\n", FILE_APPEND);

	$tour_dir = 'imgs/tours/';	
	$tour_img_file_path = $tour_dir.uniqid().".png";
	
	if(move_uploaded_file($file['tmp_name'], $tour_img_file_path))
		file_put_contents("tour_log.txt", "tour avatar file is in ".$tour_img_file_path."\n", FILE_APPEND);
	else
		file_put_contents("tour_log.txt", "tour avatar uploading failed\n", FILE_APPEND);
	
	$response = '{ "path" : "http://vvkyrychenko.zzz.com.ua/gotrip/'.$tour_img_file_path.'"}';
	file_put_contents("tour_log.txt", $response." was responsed\n", FILE_APPEND);
	
	echo $response;
?>