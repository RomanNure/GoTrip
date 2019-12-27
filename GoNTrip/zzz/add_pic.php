<html>
	<head>
		<title></title>
		<link rel = "stylesheet" href = "../../styles/main.css"/>
	</head>
	<body>
		<table border = "0px" cellspacing = "0px" cellpadding = "0px"> 
			<tr> 
				<td colspan = "3">
					<div class = "header">
						
						<?php 
							
							if(isset($_COOKIE["admin_auth"]) && $_COOKIE["admin_auth"] == "true")
								echo '<div class = "head_button" id = "new_add">Новина</div>
									  <div class = "head_button" id = "lyrics_set_add">Збірник</div>
									  <div class = "head_button" id = "lyrics_add">Вірш</div>
									  <div class = "head_button" id = "picture_add">Картина</div>
									  <div class = "head_button" id = "film_add">Фільм/Кліп</div>
									  <div class = "head_button" id = "pic_add">Завантажити картинку</div>
									  <form action = "out.php" method = "POST" id = "out_form">
										<input name = "redirect" value = "admin.php" hidden></input>
										<div class = "head_button" id = "admin_out" onclick = "document.getElementById(\'out_form\').submit();">X</div>
									  </form>';
							else 
								Header("Location: admin.php");
						?>
						
					</div>
				</td>
			</tr>
			
			<tr>
				<td style = "vertical-align : top;"><div class = "left_side"></div></td>
				<td>
					<div class = "content">
						
						<center style = 'margin-top : 30vh;'>
							<table>
								<tr>
									<td><label class = 'admin_input_label'>Файл: </label></td>
									<td><input class = 'admin_input' type = 'file' id = 'file_input' required></input></td>
								</tr>
								<tr>
									<td><label class = 'admin_input_label'>Нова назва файлу: </label></td>
									<td><input class = 'admin_input' id = 'file_name_input' oninput = "RemoveUniteQuotes('file_name_input');"></input></td>
								</tr>
								<tr>
									<td colspan = '2'>
										<center><button class = 'admin_submit_button' type = 'submit' onclick = 'upload();'>Завантажити</button></center>
									</td>
								</tr>
							</table>
						</center>
								
					</div>
				</td>
				<td style = "vertical-align : top;"><div class = "right_side"></div></td>
			</tr>
			
			<tr>
				<td colspan = "3"><div class = "footer"></div></td>
			</tr>
		</table>
		<script src = "../../scripts/js/page_switcher_admin.js"></script>
		<script src = "../../scripts/js/input_format_admin.js"></script>
		<script>
		
			function upload() {
				
				let file = document.getElementById('file_input').files[0];
				let name = document.getElementById('file_name_input').value.replace(/\.\w+/, '');
				let dir = "../../pic";
				
				let formData = new FormData();
				formData.append('dir', dir);
				formData.append('file', file, name + '.jpg');
				
				let xhr_add_pic = new XMLHttpRequest();
				xhr_add_pic.open('POST', 'upload_file.php', false);
				xhr_add_pic.send(formData);
			}
			
			function fillXMLHttpRequest(dataObject, xhr) {
				
				let boundary = String(Math.random()).slice(2);
				let boundaryMiddle = '--' + boundary + '\r\n';
				let boundaryLast = '--' + boundary + '--\r\n'

				let body = ['\r\n'];
				for (let key in dataObject) { body.push('Content-Disposition: form-data; name="' + key + '"\r\n\r\n' + dataObject[key] + '\r\n'); }

				body = body.join(boundaryMiddle) + boundaryLast;

				xhr.setRequestHeader('Content-Type', 'multipart/form-data; boundary=' + boundary);
				return body;
			}
			
		</script>
	</body>
</html>