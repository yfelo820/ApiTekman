---- SUPERCIBER ----
-- ES
UPDATE ContentBlock SET Name = 'Expresión escrita'
WHERE Id = 'D6267E68-FD02-4082-8AEF-DCB036D68F64';

-- Super content blocks
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('5867de20-530e-45c4-a773-87287c131309', 1, 'Comprensión lectora','B3A43C87-34B9-4CE2-2239-08D59AFEF162', 'BAB162B0-8756-4300-092B-08D77D882815');
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('db888b62-0d11-4345-8701-fd832688fdb9', 2, 'Expresión escrita','B3A43C87-34B9-4CE2-2239-08D59AFEF162', 'BAB162B0-8756-4300-092B-08D77D882815');

-- Comprensión lectora
UPDATE ContentBlock SET SuperContentBlockId = '5867de20-530e-45c4-a773-87287c131309'
WHERE Id = '672A19FF-4A05-47C6-B998-A21235196565';

-- Expresión escrita
UPDATE ContentBlock SET SuperContentBlockId = 'db888b62-0d11-4345-8701-fd832688fdb9'
WHERE Id = 'D6267E68-FD02-4082-8AEF-DCB036D68F64';

-- CAT
UPDATE ContentBlock SET Name = 'Expressió escrita'
WHERE Id = 'EC792A06-124C-4AAF-B1BB-13609D640934';

-- Super content blocks
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('1e345796-a05c-4f01-a904-8de38bc29feb', 1, 'Comprensió lectora','9D8DC3BE-207A-42E9-4DBC-08D5E71B75F0', 'BAB162B0-8756-4300-092B-08D77D882815');
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('4441bb58-ffbc-4ca7-ada3-f330310accf9', 2, 'Expressió escrita','9D8DC3BE-207A-42E9-4DBC-08D5E71B75F0', 'BAB162B0-8756-4300-092B-08D77D882815');

-- Comprensió lectora
UPDATE ContentBlock SET SuperContentBlockId = '1e345796-a05c-4f01-a904-8de38bc29feb'
WHERE Id = '5553CDF6-1BC4-4A3A-B62B-EA38B8EF4E82';

-- Expressió escrita
UPDATE ContentBlock SET SuperContentBlockId = '4441bb58-ffbc-4ca7-ada3-f330310accf9'
WHERE Id = 'EC792A06-124C-4AAF-B1BB-13609D640934';

-------------------------------------------

---- SUPERCIBER CAT ----
--  ES
UPDATE ContentBlock SET Name = 'Expresión escrita'
WHERE Id = 'C373D7A8-E393-4A3F-BAA0-39B473C1AA3F';

-- Super content blocks
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('efcb092a-0025-49a5-889a-2421de28c57b', 1, 'Comprensión lectora','B3A43C87-34B9-4CE2-2239-08D59AFEF162', 'AA9A19B4-BFCB-4EAB-783E-08D8FA61F718');
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('1588d697-4cfb-423d-8b62-980da95c63b3', 2, 'Expresión escrita','B3A43C87-34B9-4CE2-2239-08D59AFEF162', 'AA9A19B4-BFCB-4EAB-783E-08D8FA61F718');

-- Comprensión lectora
UPDATE ContentBlock SET SuperContentBlockId = 'efcb092a-0025-49a5-889a-2421de28c57b'
WHERE Id = '77C243D0-CDD2-4159-BF2A-8190248FFB5C';

-- Expresión escrita
UPDATE ContentBlock SET SuperContentBlockId = '1588d697-4cfb-423d-8b62-980da95c63b3'
WHERE Id = 'C373D7A8-E393-4A3F-BAA0-39B473C1AA3F';

-- CAT
UPDATE ContentBlock SET Name = 'Expressió escrita'
WHERE Id = '35FFB228-FF6D-424B-9B87-38B4B77A1AA0';

-- Super content blocks
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('8a2587fb-8beb-46f5-b703-24992bf80cba', 1, 'Comprensió lectora','9D8DC3BE-207A-42E9-4DBC-08D5E71B75F0', 'AA9A19B4-BFCB-4EAB-783E-08D8FA61F718');
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('eb32aa54-727d-4c8c-90c7-736e05a21d12', 2, 'Expressió escrita','9D8DC3BE-207A-42E9-4DBC-08D5E71B75F0', 'AA9A19B4-BFCB-4EAB-783E-08D8FA61F718');

-- Comprensió lectora
UPDATE ContentBlock SET SuperContentBlockId = '8a2587fb-8beb-46f5-b703-24992bf80cba'
WHERE Id = 'C903FCCB-1D0C-43E9-B0E5-56A3C8D7F9C2';

-- Expressió escrita
UPDATE ContentBlock SET SuperContentBlockId = 'eb32aa54-727d-4c8c-90c7-736e05a21d12'
WHERE Id = '35FFB228-FF6D-424B-9B87-38B4B77A1AA0';

-------------------------------------------

---- CIBERLUDI ----
-- ES
UPDATE ContentBlock SET Name = 'Expresión escrita'
WHERE Id = '6C409229-53EF-42FC-9047-34F4D352BAFB';

-- Super content blocks
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('4c4b6425-9abe-4abf-b05e-46c0194768d4', 1, 'Comprensión lectora','B3A43C87-34B9-4CE2-2239-08D59AFEF162', 'B8EA7E0E-24AF-4CED-0167-08D5A6A79B51');
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('8f28d114-fa14-448a-94cf-a86ad1ff04b7', 2, 'Expresión escrita','B3A43C87-34B9-4CE2-2239-08D59AFEF162', 'B8EA7E0E-24AF-4CED-0167-08D5A6A79B51');

-- Eficacia lectora to Comprensión lectora
UPDATE Activity SET ContentBlockId = 'B2A23A6E-9CF6-4665-B9B9-166960A71D85'
WHERE ContentBlockId = '0AD499FB-A8E5-4ADB-B7A1-E79D1D229496';

DELETE FROM ContentBlock WHERE Id = '0AD499FB-A8E5-4ADB-B7A1-E79D1D229496';

-- Comprensión lectora
UPDATE ContentBlock SET SuperContentBlockId = '4c4b6425-9abe-4abf-b05e-46c0194768d4'
WHERE Id = 'B2A23A6E-9CF6-4665-B9B9-166960A71D85';

-- Expresión escrita
UPDATE ContentBlock SET SuperContentBlockId = '8f28d114-fa14-448a-94cf-a86ad1ff04b7'
WHERE Id = '6C409229-53EF-42FC-9047-34F4D352BAFB';

-- MEX
UPDATE ContentBlock SET Name = 'Expresión escrita'
WHERE Id = '22722071-DD94-48B4-B614-C68171AA1731';

-- Super content blocks
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('531f953e-9c57-4b89-ab18-d60464e4f9d6', 1, 'Comprensión lectora','ED708046-2568-484D-4DBD-08D5E71B75F0', 'B8EA7E0E-24AF-4CED-0167-08D5A6A79B51');
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('c2f504a9-beb2-47a7-81d0-c26e7bb2c52e', 2, 'Expresión escrita','ED708046-2568-484D-4DBD-08D5E71B75F0', 'B8EA7E0E-24AF-4CED-0167-08D5A6A79B51');

-- Eficacia lectora to Comprensión lectora
UPDATE Activity SET ContentBlockId = 'BA623525-3DA4-4E9D-A176-A8D04EB4F469'
WHERE ContentBlockId = '4E4E5D04-0C28-4EE9-B5D9-8DBE20DAAE5A';

DELETE FROM ContentBlock WHERE Id = '4E4E5D04-0C28-4EE9-B5D9-8DBE20DAAE5A';

-- Comprensión lectora
UPDATE ContentBlock SET SuperContentBlockId = '531f953e-9c57-4b89-ab18-d60464e4f9d6'
WHERE Id = 'BA623525-3DA4-4E9D-A176-A8D04EB4F469';

-- Expresión escrita
UPDATE ContentBlock SET SuperContentBlockId = 'c2f504a9-beb2-47a7-81d0-c26e7bb2c52e'
WHERE Id = '22722071-DD94-48B4-B614-C68171AA1731';

-------------------------------------------

---- CIBERLUDI CAT ----
-- ES
UPDATE ContentBlock SET Name = 'Expresión escrita'
WHERE Id = 'C5944EEB-506B-4180-8206-92EAC17F3460';

-- Super content blocks
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('9a646d1c-95da-43fb-8f98-42cf329a6bd1', 1, 'Comprensión lectora','B3A43C87-34B9-4CE2-2239-08D59AFEF162', '3758DFFC-79AE-4511-3151-08D6B84E486B');
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('4d559ad8-6903-4322-b3e0-b983a00cba1f', 2, 'Expresión escrita','B3A43C87-34B9-4CE2-2239-08D59AFEF162', '3758DFFC-79AE-4511-3151-08D6B84E486B');

-- Eficacia lectora to Comprensión lectora
UPDATE Activity SET ContentBlockId = 'F6288BDD-E620-4A3A-8BA4-6EF0291C3783'
WHERE ContentBlockId = 'FFAE194F-BE8A-4BF4-B5A3-999901410D4C';

DELETE FROM ContentBlock WHERE Id = 'FFAE194F-BE8A-4BF4-B5A3-999901410D4C';

-- Comprensión lectora
UPDATE ContentBlock SET SuperContentBlockId = '9a646d1c-95da-43fb-8f98-42cf329a6bd1'
WHERE Id = 'F6288BDD-E620-4A3A-8BA4-6EF0291C3783';

-- Expresión escrita
UPDATE ContentBlock SET SuperContentBlockId = '4d559ad8-6903-4322-b3e0-b983a00cba1f'
WHERE Id = 'C5944EEB-506B-4180-8206-92EAC17F3460';

--  CAT
UPDATE ContentBlock SET Name = 'Expressió escrita'
WHERE Id = 'AE691561-D04B-4618-AC26-C16399839902';

-- Super content blocks
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('6a651b46-40d8-469f-ad60-a97320575aea', 1, 'Comprensió lectora','9D8DC3BE-207A-42E9-4DBC-08D5E71B75F0', '3758DFFC-79AE-4511-3151-08D6B84E486B');
INSERT INTO SuperContentBlock (Id, [Order], [Name], LanguageId, SubjectId) VALUES('a68da3b8-8476-4286-a7cb-fbaa2aee44b4', 2, 'Expressió escrita','9D8DC3BE-207A-42E9-4DBC-08D5E71B75F0', '3758DFFC-79AE-4511-3151-08D6B84E486B');

-- Eficàcia lectora to Comprensió lectora
UPDATE Activity SET ContentBlockId = 'DEF3CDD6-CAB5-41CA-A5D7-47FBEC4BB3EC'
WHERE ContentBlockId = 'D6830A3B-57A6-4754-9861-407B4B855CA3';

DELETE FROM ContentBlock WHERE Id = 'D6830A3B-57A6-4754-9861-407B4B855CA3';

-- Comprensió lectora
UPDATE ContentBlock SET SuperContentBlockId = '6a651b46-40d8-469f-ad60-a97320575aea'
WHERE Id = 'DEF3CDD6-CAB5-41CA-A5D7-47FBEC4BB3EC';

-- Expressió escrita
UPDATE ContentBlock SET SuperContentBlockId = 'a68da3b8-8476-4286-a7cb-fbaa2aee44b4'
WHERE Id = 'AE691561-D04B-4618-AC26-C16399839902';

-------------------------------------------