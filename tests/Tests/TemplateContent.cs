namespace Tests
{
    struct TemplateContent
    {
        public const string Code = @"<html>
<head>
	<title>a test</title>
</head>
<body>
	
	<p>Dear *|FNAME|*,</p>
   <p>Thank you for your purchase on *|ORDERDATE|* from ABC Widget Company. <br>
We appreciate your business and have included a copy of your invoice below. <br>
<br>
*|INVOICEDETAILS|*
<br>
Please let us know if you have further questions.


     -- ABC Widget Co.</p>

     <div mc:edit=""footer"">footer</div>
</body>
</html>";
        public const string Text = @"Dear *|FNAME|*,
   Thank you for your purchase on *|ORDERDATE|* from ABC Widget Company. 
We appreciate your business and have included a copy of your invoice below.

*|INVOICEDETAILS|*

Please let us know if you have further questions.

     -- ABC Widget Co.";
    }
}