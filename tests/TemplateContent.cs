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

        public const string HandleBarCode = @"<html>
<head>
    <title>a test</title>
</head>
<body>

    <p>Dear{{fname}},</p>
    <p>Thank you for your purchase on {{orderdate}} from ABC Widget Company. <br>
We appreciate your business and have included a copy of your invoice below. <br>
    <!-- BEGIN PRODUCT LOOP // -->
    {{#each products}}
    <tr class=""item"">
        <td valign=""top"" class=""textContent"">
            <h4 class=""itemName"">{{name}}</h4>
            <span class=""contentSecondary"">Qty: {{qty}} x ${{price}}/each</span><br />
            <span class=""contentSecondary sku""><em>{{sku}}</em></span><br />
            <span class=""contentSecondary itemDescription"">{{description}}</span>
        </td>
        <td valign=""top"" class=""textContent alignRight priceWidth"">
            ${{ordPrice}}
        </td>
    </tr>
    {{/each}}
<!-- // END PRODUCT LOOP -->
Please let us know if you have further questions.


    -- ABC Widget Co.</p>

    <div mc:edit=""footer"">footer</div>
</body>
</html>";
    }

    /*
    <!-- BEGIN PRODUCT LOOP // -->
    {{#each products}}
    <tr class="item">
        <td valign="top" class="textContent">
            <img src="{{img}}" width="50" height="75" class="itemImage" />
            <h4 class="itemName">{{name}}</h4>
            <span class="contentSecondary">Qty: {{qty}} x ${{price}}/each</span><br />
            <span class="contentSecondary sku"><em>{{sku}}</em></span><br />
            <span class="contentSecondary itemDescription">{{description}}</span>
        </td>
        <td valign="top" class="textContent alignRight priceWidth">
            ${{ordPrice}}
        </td>
    </tr>
    {{/each}}
<!-- // END PRODUCT LOOP -->

    */
}
