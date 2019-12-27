<?php
	header("Access-Control-Allow-Origin: *");
	
	foreach($_FILES as $k => $file) {
		foreach($file as $key => $val) {
			file_put_contents("user_log.txt", $key." => ".$val."\n", FILE_APPEND);
		}
	}

	$file = $_FILES['user_avatar'];	
	file_put_contents("user_log.txt", "error: ".$file['error']."\n", FILE_APPEND);

	$user_dir = 'imgs/users/';	
	$user_avatar_file_path = $user_dir.uniqid().".png";
	
	if(move_uploaded_file($file['tmp_name'], $user_avatar_file_path))
		file_put_contents("user_log.txt", "avatar file is in ".$user_avatar_file_path."\n", FILE_APPEND);
	else
		file_put_contents("user_log.txt", "avatar uploading failed\n", FILE_APPEND);
	
	$response = '{ "path" : "http://vvkyrychenko.zzz.com.ua/gotrip/'.$user_avatar_file_path.'"}';
	file_put_contents("user_log.txt", $response." was responsed\n", FILE_APPEND);
	
	echo $response;
?>