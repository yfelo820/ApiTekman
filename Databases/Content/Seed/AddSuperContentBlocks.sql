-- ESP
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('4f468d9f-bf88-4dfb-9ed9-41aeddcd20a3', 1, 'Numeración','B3A43C87-34B9-4CE2-2239-08D59AFEF162', 'D28BBC09-6F7E-44E6-3ED3-08D59BBEAA3B');
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('5eb1f0b4-49cb-4a7b-8faa-780ff8fee2c8', 2, 'Geometría','B3A43C87-34B9-4CE2-2239-08D59AFEF162', 'D28BBC09-6F7E-44E6-3ED3-08D59BBEAA3B');
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('35862bc0-9906-47a2-a6f7-5ba8dd212796', 3, 'Medida','B3A43C87-34B9-4CE2-2239-08D59AFEF162', 'D28BBC09-6F7E-44E6-3ED3-08D59BBEAA3B');
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('beae6552-b18d-4044-b79b-5c1b0ea1e489', 4, 'Estadística y probabilidad','B3A43C87-34B9-4CE2-2239-08D59AFEF162', 'D28BBC09-6F7E-44E6-3ED3-08D59BBEAA3B');

-- Numeración 
UPDATE ContentBlock SET SuperContentBlockId = '4f468d9f-bf88-4dfb-9ed9-41aeddcd20a3'
WHERE Id in ('85C04B32-2268-4A6F-8BB1-6938FD2AE32F', -- Los números
             'A7ED269E-D127-45FF-93F9-13878B9B3EB2', -- Números romanos
             '53AD91F0-BC45-4E05-877D-7B7E40549D81', -- Fracciones y decimales
             'CA2C72C9-481B-4A11-91B9-69E168BC408E', -- Numeración
             '589A1716-61FD-469A-91CF-5C9FE405D61B', -- Operaciones
             'C1E81668-B61B-4C7D-ACFC-12C0DF96BE4C', -- Resolución
             '1033CC63-8734-4E1F-8C2C-51175D7A00AE');
-- Geometría
UPDATE ContentBlock SET SuperContentBlockId = '5eb1f0b4-49cb-4a7b-8faa-780ff8fee2c8'
WHERE Id in ('DF0A488A-B745-4322-852E-2D7EEDB722C8', -- Elementos geométricos
             'A0DB18D6-6AC9-4571-B22C-6D383ED29457', -- Figuras geométricas
             '02B6EBB6-22AA-4B0D-9D40-299EFAC0135C', -- Cuerpos geométricos
             '5F148923-F7EB-478A-9A1F-1F1E8AFB5A56', -- Coordenadas y mapas
             '9EA218CA-B3CA-4E2D-8A04-0BE3DBCD6A46', -- El plano
             'EEE5DFEF-DF3F-4946-B1A3-1D6861ADBC02');  -- Cálculos geométricos

-- Medida
UPDATE ContentBlock SET SuperContentBlockId = '35862bc0-9906-47a2-a6f7-5ba8dd212796'
WHERE Id in ('6AC72826-3309-4D5E-8A68-89C0B001C480', -- Sistema monetario
             '9F13E31F-C825-46B1-B3FF-FD086B30220E', -- Longitudes
             '1325DC62-BA69-44E2-BB1F-7DD28C84CFB8', -- El tiempo
             'C1A0ED74-AB7E-4745-897A-26FAB95A6D63', -- Pesos
             '0C2623C8-E842-459D-8731-23DF92F14603', -- Capacidades
             '2F5F6BF7-AC46-4C91-96A4-1D68D0147CAA'); -- Unidades

-- Estadística y probabilidad
UPDATE ContentBlock SET SuperContentBlockId = 'beae6552-b18d-4044-b79b-5c1b0ea1e489'
WHERE Id in ('B33D734B-0D28-4232-A6AA-4A7FA2DB6197', -- Variables estadísticas
             'D477FB63-5F8B-456B-A767-2920D9B549C9', -- Probabilidad de sucesos
             '3584F15D-CEC9-4772-BFAB-CE7F8EEEA882', -- Combinatoria
             '9BE5C84C-A6A9-46E2-BF51-9D53B5588127');-- Gestión de datos

-- CAT
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('24508795-4c34-4f14-9717-cb372908ba9e', 1, 'Nombres', '9D8DC3BE-207A-42E9-4DBC-08D5E71B75F0', 'D28BBC09-6F7E-44E6-3ED3-08D59BBEAA3B');
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('9cc00027-56e8-4f80-86ac-49d961bdd756', 2, 'Relacions i canvi','9D8DC3BE-207A-42E9-4DBC-08D5E71B75F0', 'D28BBC09-6F7E-44E6-3ED3-08D59BBEAA3B');
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('920fad1a-23f1-4316-80dd-aab21e93ab22', 3, 'Espai i forma','9D8DC3BE-207A-42E9-4DBC-08D5E71B75F0', 'D28BBC09-6F7E-44E6-3ED3-08D59BBEAA3B');
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('be5da7e4-3529-4fa6-8027-060fc8904a6f', 4, 'Mesura', '9D8DC3BE-207A-42E9-4DBC-08D5E71B75F0', 'D28BBC09-6F7E-44E6-3ED3-08D59BBEAA3B');
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('1a3d165d-e505-4756-ad94-60223b162ea2', 5, 'Estadística i probabilitat','9D8DC3BE-207A-42E9-4DBC-08D5E71B75F0', 'D28BBC09-6F7E-44E6-3ED3-08D59BBEAA3B');

-- Nombres
UPDATE ContentBlock SET SuperContentBlockId = '24508795-4c34-4f14-9717-cb372908ba9e'
WHERE Id in ('B4128082-92D8-40CC-88CD-66EE9780010A', -- Els nombres
             '5B6596A4-A9BD-40B4-BE18-84B0054869B8', -- Nombres romans
             'F99E3C0C-F6CC-4593-91A7-6F32DE8658BE', -- Fraccions i decimals
             '16387B28-A766-4F7B-8E93-0312E8FB5537', -- Numeració
             'B8C91B84-888A-4E3B-8B3A-ED010378839D', -- Operacions
             '57B16620-560C-4911-9A84-93D6429904FE');-- Resolució

-- Relacions i canvi
UPDATE ContentBlock SET SuperContentBlockId = '9cc00027-56e8-4f80-86ac-49d961bdd756'
WHERE Id in ('79358B2A-0F8B-407C-B26C-0D8A579D39DA', -- Funcions
             'EDC041AC-9EAF-4D3A-88D6-39132181C36E');-- Sistema monetari

-- Espai i forma
UPDATE ContentBlock SET SuperContentBlockId = '920fad1a-23f1-4316-80dd-aab21e93ab22'
WHERE Id in ('68FC10AF-C153-4CA8-A3B2-76A23F2951BA', -- Elements geomètrics
             'A815B17C-9302-4A2D-AC7C-6628C3C0300D', -- Figures geomètriques
             'B50236C4-1D69-4C93-8410-C5592EC2B540', -- Cossos geomètrics
             '483B102F-BB28-4CEE-B5B5-47084425A8CD', -- Coordenades i mapes
             'B3BE1051-51A1-4686-ACA2-7800C5AF7DB7', -- El pla
             'BE518BC4-43CB-42AE-80B8-6D738A161491');-- Càlculs geomètrics

-- Mesura
UPDATE ContentBlock SET SuperContentBlockId = 'be5da7e4-3529-4fa6-8027-060fc8904a6f'
WHERE Id in ('6B5EBDE5-8366-427C-9208-4A5392399ED4', -- Longituds
             'EDAD5D66-6858-4590-B352-5A8100930A1E', -- El temps
             'C2FA2BD6-B209-42AB-8E51-FD5B1061AA88', -- Pesos
             'ED392E72-A275-4A8B-BCA2-DB6F6D51558A', -- Capacitats
             '61786DDE-E90D-445A-9C0B-290320F7E71D');-- Unitats

-- Estadística i probabilitat
UPDATE ContentBlock SET SuperContentBlockId = '1a3d165d-e505-4756-ad94-60223b162ea2'
WHERE Id in ('AC8FEA74-9078-4230-AD4F-4B4E5B70B491', -- Variables estadístiques
             '00C2E49E-F683-4D8B-B15F-9DCF2217D163', -- Probabilidad de successos
             '5F05562E-A65D-473D-8496-05499EF04F7E', -- Combinatòria
             '940F9C23-B089-4409-9A23-5A6CC80C776A');-- Gestió de dades

-- MEX
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('464a7c55-85ee-4ae3-860e-f1ed7f54c5cd', 1, 'Números, álgebra y variación', 'ED708046-2568-484D-4DBD-08D5E71B75F0', 'D28BBC09-6F7E-44E6-3ED3-08D59BBEAA3B');
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('93b3640f-e3ac-409a-899c-e4be9909a689', 2, 'Forma, espacio y medida','ED708046-2568-484D-4DBD-08D5E71B75F0', 'D28BBC09-6F7E-44E6-3ED3-08D59BBEAA3B');
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('2e763703-b75e-4ab6-abac-4ccace9c60fd', 3, 'Análisis de datos','ED708046-2568-484D-4DBD-08D5E71B75F0', 'D28BBC09-6F7E-44E6-3ED3-08D59BBEAA3B');

-- Números, álgebra y variación
UPDATE ContentBlock SET SuperContentBlockId = '464a7c55-85ee-4ae3-860e-f1ed7f54c5cd'
WHERE Id in ('5E4D2B0F-7988-40F8-BDDD-94D251A77B15', -- Los números
             '2EC02E88-6A49-41F8-8DC0-581AD53755FD', -- Números romanos
             'E29F586A-ABDD-4BF4-AA98-6F3695785D17', -- Fracciones y decimales
             '728C1EFC-51DA-4105-AC79-86A2C3E8EA3B', -- Numeración
             'E6D7AF95-FE67-4D64-A1B2-78A5A2505ABE', -- Operaciones
             'A8828DA0-5BAC-4DEF-B01E-8B671E5F1CF9', -- Resolución
             'CC71510E-5B43-40E3-A9E4-590267F1C5A1');-- Funciones

-- Forma, espacio y medida
UPDATE ContentBlock SET SuperContentBlockId = '93b3640f-e3ac-409a-899c-e4be9909a689'
WHERE Id in ('11DFE294-17F7-42EE-9767-08F3D99CEA5F', -- Elementos geométricos
             '86C224A4-7D2E-434D-B9FD-AEC95D2E182D', -- Figuras geométricas
             '7139000A-39A3-4A3B-AC73-9977871E60D2', -- Cuerpos geométricos
             '5B46308D-6B87-4751-BB1D-3FCF91E7C862', -- Coordenadas y mapas
             'D2B1A627-9E0A-42CA-97ED-4F72A31024CC', -- El plano
             '83B85BC9-D482-4314-9ED1-EEC8578BD136', -- Cálculos geométricos
             '36A5D4B4-7F83-48ED-BF2E-3220E8C5EE19', -- Sistema monetario
             '59C45183-870D-435F-B97D-CF1718D5849E', -- Longitudes
             '56C0A9E5-8685-4308-8E4D-43A5C796D531', -- El tiempo
             '342CC92A-86F9-4360-BE66-0AF7A1D0AC5E', -- Pesos
             '133796D3-D00E-47B0-B18E-5A4A1159E4C6', -- Capacidades
             '4234FAD2-39FB-433F-92FE-5F20DDB53804');-- Unidades

-- Análisis de datos
UPDATE ContentBlock SET SuperContentBlockId = '2e763703-b75e-4ab6-abac-4ccace9c60fd'
WHERE Id in ('C01B9CFE-2AAB-484E-868D-B46E0238B43E', -- Variables estadísticas
             '81E41FD9-C500-454F-AA7E-F71207877E05', -- Probabilidad de sucesos
             'B1BA93AE-6790-4AD2-8AD5-861159096AC2', -- Combinatoria
             '82FBCAA3-4D9B-4F3C-B67E-10DCCFF92EC3');-- Gestión de datos