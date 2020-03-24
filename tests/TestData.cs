using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    public class TestData
    {
        #region mandrill_inbound

        public const string mandrill_inbound = @"[
    {
        ""event"": ""inbound"",
        ""msg"": {
            ""dkim"": {
                ""signed"": true,
                ""valid"": true
            },
            ""email"": ""test@inbound.example.com"",
            ""from_email"": ""example.sender@mandrillapp.com"",
            ""from_name"": ""Example Sender"",
            ""headers"": {
                ""Content-Type"": ""multipart/alternative; boundary=\""_av-7r7zDhHxVEAo2yMWasfuFw\"""",
                ""Date"": ""Fri, 10 May 2013 19:28:20 +0000"",
                ""Dkim-Signature"": [
                    ""v=1; a=rsa-sha1; c=relaxed/relaxed; s=mandrill; d=mail115.us4.mandrillapp.com; h=From:Sender:Subject:List-Unsubscribe:To:Message-Id:Date:MIME-Version:Content-Type; i=example.sender@mail115.us4.mandrillapp.com; bh=d60x72jf42gLILD7IiqBL0OBb40=; b=iJd7eBugdIjzqW84UZ2xynlg1SojANJR6xfz0JDD44h78EpbqJiYVcMIfRG7mkrn741Bd5YaMR6p 9j41OA9A5am+8BE1Ng2kLRGwou5hRInn+xXBAQm2NUt5FkoXSpvm4gC4gQSOxPbQcuzlLha9JqxJ 8ZZ89/20txUrRq9cYxk="",
                    ""v=1; a=rsa-sha256; c=relaxed/relaxed; d=c.mandrillapp.com; i=@c.mandrillapp.com; q=dns/txt; s=mandrill; t=1368214100; h=From : Sender : Subject : List-Unsubscribe : To : Message-Id : Date : MIME-Version : Content-Type : From : Subject : Date : X-Mandrill-User : List-Unsubscribe; bh=y5Vz+RDcKZmWzRc9s0xUJR6k4APvBNktBqy1EhSWM8o=; b=PLAUIuw8zk8kG5tPkmcnSanElxt6I5lp5t32nSvzVQE7R8u0AmIEjeIDZEt31+Q9PWt+nY sHHRsXUQ9SZpndT9Bk++/SmyA2ntU/2AKuqDpPkMZiTqxmGF80Wz4JJgx2aCEB1LeLVmFFwB 5Nr/LBSlsBlRcjT9QiWw0/iRvCn74=""
                ],
                ""Domainkey-Signature"": ""a=rsa-sha1; c=nofws; q=dns; s=mandrill; d=mail115.us4.mandrillapp.com; b=X6qudHd4oOJvVQZcoAEUCJgB875SwzEO5UKf6NvpfqyCVjdaO79WdDulLlfNVELeuoK2m6alt2yw 5Qhp4TW5NegyFf6Ogr/Hy0Lt411r/0lRf0nyaVkqMM/9g13B6D9CS092v70wshX8+qdyxK8fADw8 kEelbCK2cEl0AGIeAeo=;"",
                ""From"": ""<example.sender@mandrillapp.com>"",
                ""List-Unsubscribe"": ""<mailto:unsubscribe-md_999.aaaaaaaa.v1-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@mailin1.us2.mcsv.net?subject=unsub>"",
                ""Message-Id"": ""<999.20130510192820.aaaaaaaaaaaaaa.aaaaaaaa@mail115.us4.mandrillapp.com>"",
                ""Mime-Version"": ""1.0"",
                ""Received"": [
                    ""from mail115.us4.mandrillapp.com (mail115.us4.mandrillapp.com [205.201.136.115]) by mail.example.com (Postfix) with ESMTP id AAAAAAAAAAA for <test@inbound.example.com>; Fri, 10 May 2013 19:28:21 +0000 (UTC)"",
                    ""from localhost (127.0.0.1) by mail115.us4.mandrillapp.com id hhl55a14i282 for <test@inbound.example.com>; Fri, 10 May 2013 19:28:20 +0000 (envelope-from <bounce-md_999.aaaaaaaa.v1-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@mail115.us4.mandrillapp.com>)""
                ],
                ""Sender"": ""<example.sender@mail115.us4.mandrillapp.com>"",
                ""Subject"": ""This is an example webhook message"",
                ""To"": ""<test@inbound.example.com>"",
                ""X-Report-Abuse"": ""Please forward a copy of this message, including all headers, to abuse@mandrill.com""
            },
            ""html"": ""<p>This is an example inbound message.</p><img src=\""http://mandrillapp.com/track/open.php?u=999&id=aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa&tags=_all,_sendexample.sender@mandrillapp.com\"" height=\""1\"" width=\""1\"">\n"",
            ""raw_msg"": ""Received: from mail115.us4.mandrillapp.com (mail115.us4.mandrillapp.com [205.201.136.115])\n\tby mail.example.com (Postfix) with ESMTP id AAAAAAAAAAA\n\tfor <test@inbound.example.com>; Fri, 10 May 2013 19:28:20 +0000 (UTC)\nDKIM-Signature: v=1; a=rsa-sha1; c=relaxed/relaxed; s=mandrill; d=mail115.us4.mandrillapp.com;\n h=From:Sender:Subject:List-Unsubscribe:To:Message-Id:Date:MIME-Version:Content-Type; i=example.sender@mail115.us4.mandrillapp.com;\n bh=d60x72jf42gLILD7IiqBL0OBb40=;\n b=iJd7eBugdIjzqW84UZ2xynlg1SojANJR6xfz0JDD44h78EpbqJiYVcMIfRG7mkrn741Bd5YaMR6p\n 9j41OA9A5am+8BE1Ng2kLRGwou5hRInn+xXBAQm2NUt5FkoXSpvm4gC4gQSOxPbQcuzlLha9JqxJ\n 8ZZ89/20txUrRq9cYxk=\nDomainKey-Signature: a=rsa-sha1; c=nofws; q=dns; s=mandrill; d=mail115.us4.mandrillapp.com;\n b=X6qudHd4oOJvVQZcoAEUCJgB875SwzEO5UKf6NvpfqyCVjdaO79WdDulLlfNVELeuoK2m6alt2yw\n 5Qhp4TW5NegyFf6Ogr/Hy0Lt411r/0lRf0nyaVkqMM/9g13B6D9CS092v70wshX8+qdyxK8fADw8\n kEelbCK2cEl0AGIeAeo=;\nReceived: from localhost (127.0.0.1) by mail115.us4.mandrillapp.com id hhl55a14i282 for <test@inbound.example.com>; Fri, 10 May 2013 19:28:20 +0000 (envelope-from <bounce-md_999.aaaaaaaa.v1-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@mail115.us4.mandrillapp.com>)\nDKIM-Signature: v=1; a=rsa-sha256; c=relaxed/relaxed; d=c.mandrillapp.com; \n i=@c.mandrillapp.com; q=dns/txt; s=mandrill; t=1368214100; h=From : \n Sender : Subject : List-Unsubscribe : To : Message-Id : Date : \n MIME-Version : Content-Type : From : Subject : Date : X-Mandrill-User : \n List-Unsubscribe; bh=y5Vz+RDcKZmWzRc9s0xUJR6k4APvBNktBqy1EhSWM8o=; \n b=PLAUIuw8zk8kG5tPkmcnSanElxt6I5lp5t32nSvzVQE7R8u0AmIEjeIDZEt31+Q9PWt+nY\n sHHRsXUQ9SZpndT9Bk++/SmyA2ntU/2AKuqDpPkMZiTqxmGF80Wz4JJgx2aCEB1LeLVmFFwB\n 5Nr/LBSlsBlRcjT9QiWw0/iRvCn74=\nFrom: <example.sender@mandrillapp.com>\nSender: <example.sender@mail115.us4.mandrillapp.com>\nSubject: This is an example webhook message\nList-Unsubscribe: <mailto:unsubscribe-md_999.aaaaaaaa.v1-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@mailin1.us2.mcsv.net?subject=unsub>\nTo: <test@inbound.example.com>\nX-Report-Abuse: Please forward a copy of this message, including all headers, to abuse@mandrill.com\nX-Mandrill-User: md_999\nMessage-Id: <999.20130510192820.aaaaaaaaaaaaaa.aaaaaaaa@mail115.us4.mandrillapp.com>\nDate: Fri, 10 May 2013 19:28:20 +0000\nMIME-Version: 1.0\nContent-Type: multipart/alternative; boundary=\""_av-7r7zDhHxVEAo2yMWasfuFw\""\n\n--_av-7r7zDhHxVEAo2yMWasfuFw\nContent-Type: text/plain; charset=utf-8\nContent-Transfer-Encoding: 7bit\n\nThis is an example inbound message.\n--_av-7r7zDhHxVEAo2yMWasfuFw\nContent-Type: text/html; charset=utf-8\nContent-Transfer-Encoding: 7bit\n\n<p>This is an example inbound message.</p><img src=\""http://mandrillapp.com/track/open.php?u=999&id=aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa&tags=_all,_sendexample.sender@mandrillapp.com\"" height=\""1\"" width=\""1\"">\n--_av-7r7zDhHxVEAo2yMWasfuFw--"",
            ""sender"": null,
            ""spam_report"": {
                ""matched_rules"": [
                    {
                        ""description"": ""RBL: Sender listed at http://www.dnswl.org/, no"",
                        ""name"": ""RCVD_IN_DNSWL_NONE"",
                        ""score"": 0
                    },
                    {
                        ""description"": null,
                        ""name"": null,
                        ""score"": 0
                    },
                    {
                        ""description"": ""in iadb.isipp.com]"",
                        ""name"": ""listed"",
                        ""score"": 0
                    },
                    {
                        ""description"": ""RBL: Participates in the IADB system"",
                        ""name"": ""RCVD_IN_IADB_LISTED"",
                        ""score"": -0.40000000000000002
                    },
                    {
                        ""description"": ""RBL: ISIPP IADB lists as vouched-for sender"",
                        ""name"": ""RCVD_IN_IADB_VOUCHED"",
                        ""score"": -2.2000000000000002
                    },
                    {
                        ""description"": ""RBL: IADB: Sender publishes SPF record"",
                        ""name"": ""RCVD_IN_IADB_SPF"",
                        ""score"": 0
                    },
                    {
                        ""description"": ""RBL: IADB: Sender publishes Sender ID record"",
                        ""name"": ""RCVD_IN_IADB_SENDERID"",
                        ""score"": 0
                    },
                    {
                        ""description"": ""RBL: IADB: Sender publishes Domain Keys record"",
                        ""name"": ""RCVD_IN_IADB_DK"",
                        ""score"": -0.20000000000000001
                    },
                    {
                        ""description"": ""RBL: IADB: Sender has reverse DNS record"",
                        ""name"": ""RCVD_IN_IADB_RDNS"",
                        ""score"": -0.20000000000000001
                    },
                    {
                        ""description"": ""SPF: HELO matches SPF record"",
                        ""name"": ""SPF_HELO_PASS"",
                        ""score"": 0
                    },
                    {
                        ""description"": ""BODY: HTML included in message"",
                        ""name"": ""HTML_MESSAGE"",
                        ""score"": 0
                    },
                    {
                        ""description"": ""BODY: HTML: images with 0-400 bytes of words"",
                        ""name"": ""HTML_IMAGE_ONLY_04"",
                        ""score"": 0.29999999999999999
                    },
                    {
                        ""description"": ""Message has a DKIM or DK signature, not necessarily valid"",
                        ""name"": ""DKIM_SIGNED"",
                        ""score"": 0.10000000000000001
                    },
                    {
                        ""description"": ""Message has at least one valid DKIM or DK signature"",
                        ""name"": ""DKIM_VALID"",
                        ""score"": -0.10000000000000001
                    }
                ],
                ""score"": -2.6000000000000001
            },
            ""spf"": {
                ""detail"": ""sender SPF authorized"",
                ""result"": ""pass""
            },
            ""subject"": ""This is an example webhook message"",
            ""tags"": [],
            ""template"": null,
            ""text"": ""This is an example inbound message.\n"",
            ""text_flowed"": false,
            ""to"": [
                        [
                            ""test@inbound.example.com"",
                            null
                        ]
            ],
            ""cc"": [
                        [
                            ""testCc@inbound.example.com"",
                            null
                        ]
            ]
        },
        ""ts"": 1368214102
    },
    {
        ""event"": ""inbound"",
        ""msg"": {
            ""dkim"": {
                ""signed"": true,
                ""valid"": true
            },
            ""email"": ""test@inbound.example.com"",
            ""from_email"": ""example.sender@mandrillapp.com"",
            ""from_name"": null,
            ""headers"": {
                ""Content-Type"": ""multipart/alternative; boundary=\""_av-7r7zDhHxVEAo2yMWasfuFw\"""",
                ""Date"": ""Fri, 10 May 2013 19:28:20 +0000"",
                ""Dkim-Signature"": [
                    ""v=1; a=rsa-sha1; c=relaxed/relaxed; s=mandrill; d=mail115.us4.mandrillapp.com; h=From:Sender:Subject:List-Unsubscribe:To:Message-Id:Date:MIME-Version:Content-Type; i=example.sender@mail115.us4.mandrillapp.com; bh=d60x72jf42gLILD7IiqBL0OBb40=; b=iJd7eBugdIjzqW84UZ2xynlg1SojANJR6xfz0JDD44h78EpbqJiYVcMIfRG7mkrn741Bd5YaMR6p 9j41OA9A5am+8BE1Ng2kLRGwou5hRInn+xXBAQm2NUt5FkoXSpvm4gC4gQSOxPbQcuzlLha9JqxJ 8ZZ89/20txUrRq9cYxk="",
                    ""v=1; a=rsa-sha256; c=relaxed/relaxed; d=c.mandrillapp.com; i=@c.mandrillapp.com; q=dns/txt; s=mandrill; t=1368214100; h=From : Sender : Subject : List-Unsubscribe : To : Message-Id : Date : MIME-Version : Content-Type : From : Subject : Date : X-Mandrill-User : List-Unsubscribe; bh=y5Vz+RDcKZmWzRc9s0xUJR6k4APvBNktBqy1EhSWM8o=; b=PLAUIuw8zk8kG5tPkmcnSanElxt6I5lp5t32nSvzVQE7R8u0AmIEjeIDZEt31+Q9PWt+nY sHHRsXUQ9SZpndT9Bk++/SmyA2ntU/2AKuqDpPkMZiTqxmGF80Wz4JJgx2aCEB1LeLVmFFwB 5Nr/LBSlsBlRcjT9QiWw0/iRvCn74=""
                ],
                ""Domainkey-Signature"": ""a=rsa-sha1; c=nofws; q=dns; s=mandrill; d=mail115.us4.mandrillapp.com; b=X6qudHd4oOJvVQZcoAEUCJgB875SwzEO5UKf6NvpfqyCVjdaO79WdDulLlfNVELeuoK2m6alt2yw 5Qhp4TW5NegyFf6Ogr/Hy0Lt411r/0lRf0nyaVkqMM/9g13B6D9CS092v70wshX8+qdyxK8fADw8 kEelbCK2cEl0AGIeAeo=;"",
                ""From"": ""<example.sender@mandrillapp.com>"",
                ""List-Unsubscribe"": ""<mailto:unsubscribe-md_999.aaaaaaaa.v1-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@mailin1.us2.mcsv.net?subject=unsub>"",
                ""Message-Id"": ""<999.20130510192820.aaaaaaaaaaaaaa.aaaaaaaa@mail115.us4.mandrillapp.com>"",
                ""Mime-Version"": ""1.0"",
                ""Received"": [
                    ""from mail115.us4.mandrillapp.com (mail115.us4.mandrillapp.com [205.201.136.115]) by mail.example.com (Postfix) with ESMTP id AAAAAAAAAAA for <test@inbound.example.com>; Fri, 10 May 2013 19:28:21 +0000 (UTC)"",
                    ""from localhost (127.0.0.1) by mail115.us4.mandrillapp.com id hhl55a14i282 for <test@inbound.example.com>; Fri, 10 May 2013 19:28:20 +0000 (envelope-from <bounce-md_999.aaaaaaaa.v1-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@mail115.us4.mandrillapp.com>)""
                ],
                ""Sender"": ""<example.sender@mail115.us4.mandrillapp.com>"",
                ""Subject"": ""This is an example webhook message"",
                ""To"": ""<test@inbound.example.com>"",
                ""X-Report-Abuse"": ""Please forward a copy of this message, including all headers, to abuse@mandrill.com""
            },
            ""html"": ""<p>This is an example inbound message.</p><img src=\""http://mandrillapp.com/track/open.php?u=999&id=aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa&tags=_all,_sendexample.sender@mandrillapp.com\"" height=\""1\"" width=\""1\"">\n"",
            ""raw_msg"": ""Received: from mail115.us4.mandrillapp.com (mail115.us4.mandrillapp.com [205.201.136.115])\n\tby mail.example.com (Postfix) with ESMTP id AAAAAAAAAAA\n\tfor <test@inbound.example.com>; Fri, 10 May 2013 19:28:20 +0000 (UTC)\nDKIM-Signature: v=1; a=rsa-sha1; c=relaxed/relaxed; s=mandrill; d=mail115.us4.mandrillapp.com;\n h=From:Sender:Subject:List-Unsubscribe:To:Message-Id:Date:MIME-Version:Content-Type; i=example.sender@mail115.us4.mandrillapp.com;\n bh=d60x72jf42gLILD7IiqBL0OBb40=;\n b=iJd7eBugdIjzqW84UZ2xynlg1SojANJR6xfz0JDD44h78EpbqJiYVcMIfRG7mkrn741Bd5YaMR6p\n 9j41OA9A5am+8BE1Ng2kLRGwou5hRInn+xXBAQm2NUt5FkoXSpvm4gC4gQSOxPbQcuzlLha9JqxJ\n 8ZZ89/20txUrRq9cYxk=\nDomainKey-Signature: a=rsa-sha1; c=nofws; q=dns; s=mandrill; d=mail115.us4.mandrillapp.com;\n b=X6qudHd4oOJvVQZcoAEUCJgB875SwzEO5UKf6NvpfqyCVjdaO79WdDulLlfNVELeuoK2m6alt2yw\n 5Qhp4TW5NegyFf6Ogr/Hy0Lt411r/0lRf0nyaVkqMM/9g13B6D9CS092v70wshX8+qdyxK8fADw8\n kEelbCK2cEl0AGIeAeo=;\nReceived: from localhost (127.0.0.1) by mail115.us4.mandrillapp.com id hhl55a14i282 for <test@inbound.example.com>; Fri, 10 May 2013 19:28:20 +0000 (envelope-from <bounce-md_999.aaaaaaaa.v1-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@mail115.us4.mandrillapp.com>)\nDKIM-Signature: v=1; a=rsa-sha256; c=relaxed/relaxed; d=c.mandrillapp.com; \n i=@c.mandrillapp.com; q=dns/txt; s=mandrill; t=1368214100; h=From : \n Sender : Subject : List-Unsubscribe : To : Message-Id : Date : \n MIME-Version : Content-Type : From : Subject : Date : X-Mandrill-User : \n List-Unsubscribe; bh=y5Vz+RDcKZmWzRc9s0xUJR6k4APvBNktBqy1EhSWM8o=; \n b=PLAUIuw8zk8kG5tPkmcnSanElxt6I5lp5t32nSvzVQE7R8u0AmIEjeIDZEt31+Q9PWt+nY\n sHHRsXUQ9SZpndT9Bk++/SmyA2ntU/2AKuqDpPkMZiTqxmGF80Wz4JJgx2aCEB1LeLVmFFwB\n 5Nr/LBSlsBlRcjT9QiWw0/iRvCn74=\nFrom: <example.sender@mandrillapp.com>\nSender: <example.sender@mail115.us4.mandrillapp.com>\nSubject: This is an example webhook message\nList-Unsubscribe: <mailto:unsubscribe-md_999.aaaaaaaa.v1-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@mailin1.us2.mcsv.net?subject=unsub>\nTo: <test@inbound.example.com>\nX-Report-Abuse: Please forward a copy of this message, including all headers, to abuse@mandrill.com\nX-Mandrill-User: md_999\nMessage-Id: <999.20130510192820.aaaaaaaaaaaaaa.aaaaaaaa@mail115.us4.mandrillapp.com>\nDate: Fri, 10 May 2013 19:28:20 +0000\nMIME-Version: 1.0\nContent-Type: multipart/alternative; boundary=\""_av-7r7zDhHxVEAo2yMWasfuFw\""\n\n--_av-7r7zDhHxVEAo2yMWasfuFw\nContent-Type: text/plain; charset=utf-8\nContent-Transfer-Encoding: 7bit\n\nThis is an example inbound message.\n--_av-7r7zDhHxVEAo2yMWasfuFw\nContent-Type: text/html; charset=utf-8\nContent-Transfer-Encoding: 7bit\n\n<p>This is an example inbound message.</p><img src=\""http://mandrillapp.com/track/open.php?u=999&id=aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa&tags=_all,_sendexample.sender@mandrillapp.com\"" height=\""1\"" width=\""1\"">\n--_av-7r7zDhHxVEAo2yMWasfuFw--"",
            ""sender"": null,
            ""attachments"": {
                ""file.txt"": {
                    ""name"": ""file.txt"",
                    ""type"": ""plain/text"",
                    ""content"": ""123""
                }
            },
            ""images"": {
                ""ss2.png"": {
                    ""name"": ""ss2.png"",
                    ""type"": ""image/png"",
                    ""content"": ""R0lGODlhAQABAIAAAP///wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==""
                }
            },
            ""spam_report"": {
                ""matched_rules"": [
                    {
                        ""description"": ""RBL: Sender listed at http://www.dnswl.org/, no"",
                        ""name"": ""RCVD_IN_DNSWL_NONE"",
                        ""score"": 0
                    },
                    {
                        ""description"": null,
                        ""name"": null,
                        ""score"": 0
                    },
                    {
                        ""description"": ""in iadb.isipp.com]"",
                        ""name"": ""listed"",
                        ""score"": 0
                    },
                    {
                        ""description"": ""RBL: Participates in the IADB system"",
                        ""name"": ""RCVD_IN_IADB_LISTED"",
                        ""score"": -0.40000000000000002
                    },
                    {
                        ""description"": ""RBL: ISIPP IADB lists as vouched-for sender"",
                        ""name"": ""RCVD_IN_IADB_VOUCHED"",
                        ""score"": -2.2000000000000002
                    },
                    {
                        ""description"": ""RBL: IADB: Sender publishes SPF record"",
                        ""name"": ""RCVD_IN_IADB_SPF"",
                        ""score"": 0
                    },
                    {
                        ""description"": ""RBL: IADB: Sender publishes Sender ID record"",
                        ""name"": ""RCVD_IN_IADB_SENDERID"",
                        ""score"": 0
                    },
                    {
                        ""description"": ""RBL: IADB: Sender publishes Domain Keys record"",
                        ""name"": ""RCVD_IN_IADB_DK"",
                        ""score"": -0.20000000000000001
                    },
                    {
                        ""description"": ""RBL: IADB: Sender has reverse DNS record"",
                        ""name"": ""RCVD_IN_IADB_RDNS"",
                        ""score"": -0.20000000000000001
                    },
                    {
                        ""description"": ""SPF: HELO matches SPF record"",
                        ""name"": ""SPF_HELO_PASS"",
                        ""score"": 0
                    },
                    {
                        ""description"": ""BODY: HTML included in message"",
                        ""name"": ""HTML_MESSAGE"",
                        ""score"": 0
                    },
                    {
                        ""description"": ""BODY: HTML: images with 0-400 bytes of words"",
                        ""name"": ""HTML_IMAGE_ONLY_04"",
                        ""score"": 0.29999999999999999
                    },
                    {
                        ""description"": ""Message has a DKIM or DK signature, not necessarily valid"",
                        ""name"": ""DKIM_SIGNED"",
                        ""score"": 0.10000000000000001
                    },
                    {
                        ""description"": ""Message has at least one valid DKIM or DK signature"",
                        ""name"": ""DKIM_VALID"",
                        ""score"": -0.10000000000000001
                    }
                ],
                ""score"": -2.6000000000000001
            },
            ""spf"": {
                ""detail"": ""sender SPF authorized"",
                ""result"": ""pass""
            },
            ""subject"": ""This is an example webhook message"",
            ""tags"": [],
            ""template"": null,
            ""text"": ""This is an example inbound message.\n"",
            ""text_flowed"": false,
            ""to"": [
                [
                    ""test@inbound.example.com"",
                    null
                ]
            ],
            ""cc"": [
                        [
                            ""testCc@inbound.example.com"",
                            null
                        ]
            ]
        },
        ""ts"": 1368214102
    }
]";

        #endregion

        #region mandrill_webhook_example

        public const string mandrill_webhook_example = @"[
  {
    ""event"": ""send"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""opens"": [],
    ""clicks"": [],
    ""state"": ""sent"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa""
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ts"": 1420303407
  },
  {
    ""event"": ""deferral"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""opens"": [],
    ""clicks"": [],
    ""state"": ""deferred"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa"",
    ""smtp_events"": [
        {
        ""destination_ip"": ""127.0.0.1"",
        ""diag"": ""451 4.3.5 Temporarily unavailable, try again later."",
        ""source_ip"": ""127.0.0.1"",
        ""ts"": 1365111111,
        ""type"": ""deferred"",
        ""size"": 0
        }
    ]
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ts"": 1420303407
  },
  {
    ""event"": ""hard_bounce"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""state"": ""bounced"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa"",
    ""bounce_description"": ""bad_mailbox"",
    ""bgtools_code"": 10,
    ""diag"": ""smtp;550 5.1.1 The email account that you tried to reach does not exist. Please try double-checking the recipient's email address for typos or unnecessary spaces.""
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ts"": 1420303407
  },
  {
    ""event"": ""soft_bounce"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""state"": ""soft-bounced"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa"",
    ""bounce_description"": ""mailbox_full"",
    ""bgtools_code"": 22,
    ""diag"": ""smtp;552 5.2.2 Over Quota""
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ts"": 1420303407
  },
  {
    ""event"": ""open"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""opens"": [
        {
        ""ts"": 1365111111
        }
    ],
    ""clicks"": [
        {
        ""ts"": 1365111111,
        ""url"": ""http://mandrill.com""
        }
    ],
    ""state"": ""sent"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa""
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ip"": ""127.0.0.1"",
    ""location"": {
    ""country_short"": ""US"",
    ""country"": ""United States"",
    ""region"": ""Oklahoma"",
    ""city"": ""Oklahoma City"",
    ""latitude"": 35.4675598145,
    ""longitude"": -97.5164337158,
    ""postal_code"": ""73101"",
    ""timezone"": ""-05:00""
    },
    ""user_agent"": ""Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.6; en-US; rv:1.9.1.8) Gecko/20100317 Postbox/1.1.3"",
    ""user_agent_parsed"": {
    ""type"": ""Email Client"",
    ""ua_family"": ""Postbox"",
    ""ua_name"": ""Postbox 1.1.3"",
    ""ua_version"": ""1.1.3"",
    ""ua_url"": ""http://www.postbox-inc.com/"",
    ""ua_company"": ""Postbox, Inc."",
    ""ua_company_url"": ""http://www.postbox-inc.com/"",
    ""ua_icon"": ""http://cdn.mandrill.com/img/email-client-icons/postbox.png"",
    ""os_family"": ""OS X"",
    ""os_name"": ""OS X 10.6 Snow Leopard"",
    ""os_url"": ""http://www.apple.com/osx/"",
    ""os_company"": ""Apple Computer, Inc."",
    ""os_company_url"": ""http://www.apple.com/"",
    ""os_icon"": ""http://cdn.mandrill.com/img/email-client-icons/macosx.png"",
    ""mobile"": false
    },
    ""ts"": 1420303407
  },
  {
    ""event"": ""click"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""opens"": [
        {
        ""ts"": 1365111111
        }
    ],
    ""clicks"": [
        {
        ""ts"": 1365111111,
        ""url"": ""http://mandrill.com""
        }
    ],
    ""state"": ""sent"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa""
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ip"": ""127.0.0.1"",
    ""location"": {
    ""country_short"": ""US"",
    ""country"": ""United States"",
    ""region"": ""Oklahoma"",
    ""city"": ""Oklahoma City"",
    ""latitude"": 35.4675598145,
    ""longitude"": -97.5164337158,
    ""postal_code"": ""73101"",
    ""timezone"": ""-05:00""
    },
    ""user_agent"": ""Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.6; en-US; rv:1.9.1.8) Gecko/20100317 Postbox/1.1.3"",
    ""user_agent_parsed"": {
    ""type"": ""Email Client"",
    ""ua_family"": ""Postbox"",
    ""ua_name"": ""Postbox 1.1.3"",
    ""ua_version"": ""1.1.3"",
    ""ua_url"": ""http://www.postbox-inc.com/"",
    ""ua_company"": ""Postbox, Inc."",
    ""ua_company_url"": ""http://www.postbox-inc.com/"",
    ""ua_icon"": ""http://cdn.mandrill.com/img/email-client-icons/postbox.png"",
    ""os_family"": ""OS X"",
    ""os_name"": ""OS X 10.6 Snow Leopard"",
    ""os_url"": ""http://www.apple.com/osx/"",
    ""os_company"": ""Apple Computer, Inc."",
    ""os_company_url"": ""http://www.apple.com/"",
    ""os_icon"": ""http://cdn.mandrill.com/img/email-client-icons/macosx.png"",
    ""mobile"": false
    },
    ""url"": ""http://mandrill.com"",
    ""ts"": 1420303407
  },
  {
    ""event"": ""spam"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""opens"": [
        {
        ""ts"": 1365111111
        }
    ],
    ""clicks"": [
        {
        ""ts"": 1365111111,
        ""url"": ""http://mandrill.com""
        }
    ],
    ""state"": ""sent"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa""
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ts"": 1420303407
  },
  {
    ""event"": ""unsub"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""opens"": [
        {
        ""ts"": 1365111111
        }
    ],
    ""clicks"": [
        {
        ""ts"": 1365111111,
        ""url"": ""http://mandrill.com""
        }
    ],
    ""state"": ""sent"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa""
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ts"": 1420303407
  },
  {
    ""event"": ""reject"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""opens"": [],
    ""clicks"": [],
    ""state"": ""rejected"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa""
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ts"": 1420303407
  },
  {
    ""type"": ""blacklist"",
    ""action"": ""add"",
    ""reject"": {
    ""reason"": ""hard-bounce"",
    ""detail"": ""Example detail"",
    ""last_event_at"": ""2014-02-01 12:43:56"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""created_at"": ""2014-01-15 11:32:19"",
    ""expires_at"": ""2020-04-02 12:09:18"",
    ""expired"": false,
    ""subaccount"": ""example_subaccount"",
    ""sender"": ""example.sender@mandrillapp.com""
    },
    ""ts"": 1420303407
  },
  {
    ""type"": ""blacklist"",
    ""action"": ""change"",
    ""reject"": {
    ""reason"": ""hard-bounce"",
    ""detail"": ""Example detail"",
    ""last_event_at"": ""2014-02-01 12:43:56"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""created_at"": ""2014-01-15 11:32:19"",
    ""expires_at"": ""2020-04-02 12:09:18"",
    ""expired"": false,
    ""subaccount"": ""example_subaccount"",
    ""sender"": ""example.sender@mandrillapp.com""
    },
    ""ts"": 1420303407
  },
  {
    ""type"": ""blacklist"",
    ""action"": ""remove"",
    ""reject"": {
    ""reason"": ""hard-bounce"",
    ""detail"": ""Example detail"",
    ""last_event_at"": ""2014-02-01 12:43:56"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""created_at"": ""2014-01-15 11:32:19"",
    ""expires_at"": ""2020-04-02 12:09:18"",
    ""expired"": false,
    ""subaccount"": ""example_subaccount"",
    ""sender"": ""example.sender@mandrillapp.com""
    },
    ""ts"": 1420303407
  },
  {
    ""type"": ""whitelist"",
    ""action"": ""add"",
    ""entry"": {
    ""email"": ""example.webhook@mandrillapp.com"",
    ""detail"": ""example details"",
    ""created_at"": ""2014-01-15 12:03:19""
    },
    ""ts"": 1420303407
  },
  {
    ""type"": ""whitelist"",
    ""action"": ""remove"",
    ""entry"": {
    ""email"": ""example.webhook@mandrillapp.com"",
    ""detail"": ""example details"",
    ""created_at"": ""2014-01-15 12:03:19""
    },
    ""ts"": 1420303407
  }
]";

        #endregion

        #region mandrill_webhook_invalid

        public const string mandrill_webhook_invalid = @"[
  {
    ""event"": ""send"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""opens"": [],
    ""clicks"": [],
    ""state"": ""sent"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa""
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ts"": 1420303407
  },
  {
    ""event"": ""deferral"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""opens"": [],
    ""clicks"": [],
    ""state"": ""deferred"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa"",
    ""smtp_events"": [
        {
        ""destination_ip"": ""127.0.0.1"",
        ""diag"": ""451 4.3.5 Temporarily unavailable, try again later."",
        ""source_ip"": ""127.0.0.1"",
        ""ts"": 1365111111,
        ""type"": ""deferred"",
        ""size"": 0
        }
    ]
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ts"": 1420303407
  },
  {
    ""event"": ""hard_bounce"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""state"": ""bounced"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa"",
    ""bounce_description"": ""bad_mailbox"",
    ""bgtools_code"": 10,
    ""diag"": ""smtp;550 5.1.1 The email account that you tried to reach does not exist. Please try double-checking the recipient's email address for typos or unnecessary spaces.""
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ts"": 1420303407
  },
  {
    ""event"": ""soft_bounce"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""state"": ""soft-bounced"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa"",
    ""bounce_description"": ""mailbox_full"",
    ""bgtools_code"": 22,
    ""diag"": ""smtp;552 5.2.2 Over Quota""
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ts"": 1420303407
  },
  {
    ""event"": ""open"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""opens"": [
        {
        ""ts"": 1365111111
        }
    ],
    ""clicks"": [
        {
        ""ts"": 1365111111,
        ""url"": ""http://mandrill.com""
        }
    ],
    ""state"": ""sent"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa""
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ip"": ""127.0.0.1"",
    ""location"": {
    ""country_short"": ""US"",
    ""country"": ""United States"",
    ""region"": ""Oklahoma"",
    ""city"": ""Oklahoma City"",
    ""latitude"":""INVALID IP ADDRESS"",
    ""longitude"": ""INVALID IP ADDRESS"",
    ""postal_code"": ""73101"",
    ""timezone"": ""-05:00""
    },
    ""user_agent"": ""Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.6; en-US; rv:1.9.1.8) Gecko/20100317 Postbox/1.1.3"",
    ""user_agent_parsed"": {
    ""type"": ""Email Client"",
    ""ua_family"": ""Postbox"",
    ""ua_name"": ""Postbox 1.1.3"",
    ""ua_version"": ""1.1.3"",
    ""ua_url"": ""http://www.postbox-inc.com/"",
    ""ua_company"": ""Postbox, Inc."",
    ""ua_company_url"": ""http://www.postbox-inc.com/"",
    ""ua_icon"": ""http://cdn.mandrill.com/img/email-client-icons/postbox.png"",
    ""os_family"": ""OS X"",
    ""os_name"": ""OS X 10.6 Snow Leopard"",
    ""os_url"": ""http://www.apple.com/osx/"",
    ""os_company"": ""Apple Computer, Inc."",
    ""os_company_url"": ""http://www.apple.com/"",
    ""os_icon"": ""http://cdn.mandrill.com/img/email-client-icons/macosx.png"",
    ""mobile"": false
    },
    ""ts"": 1420303407
  },
  {
    ""event"": ""click"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""opens"": [
        {
        ""ts"": 1365111111
        }
    ],
    ""clicks"": [
        {
        ""ts"": 1365111111,
        ""url"": ""http://mandrill.com""
        }
    ],
    ""state"": ""sent"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa""
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ip"": ""127.0.0.1"",
    ""location"": {
    ""country_short"": ""US"",
    ""country"": ""United States"",
    ""region"": ""Oklahoma"",
    ""city"": ""Oklahoma City"",
    ""latitude"": ""INVALID IP ADDRESS"",
    ""longitude"": ""INVALID IP ADDRESS"",
    ""postal_code"": ""73101"",
    ""timezone"": ""-05:00""
    },
    ""user_agent"": ""Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.6; en-US; rv:1.9.1.8) Gecko/20100317 Postbox/1.1.3"",
    ""user_agent_parsed"": {
    ""type"": ""Email Client"",
    ""ua_family"": ""Postbox"",
    ""ua_name"": ""Postbox 1.1.3"",
    ""ua_version"": ""1.1.3"",
    ""ua_url"": ""http://www.postbox-inc.com/"",
    ""ua_company"": ""Postbox, Inc."",
    ""ua_company_url"": ""http://www.postbox-inc.com/"",
    ""ua_icon"": ""http://cdn.mandrill.com/img/email-client-icons/postbox.png"",
    ""os_family"": ""OS X"",
    ""os_name"": ""OS X 10.6 Snow Leopard"",
    ""os_url"": ""http://www.apple.com/osx/"",
    ""os_company"": ""Apple Computer, Inc."",
    ""os_company_url"": ""http://www.apple.com/"",
    ""os_icon"": ""http://cdn.mandrill.com/img/email-client-icons/macosx.png"",
    ""mobile"": false
    },
    ""url"": ""http://mandrill.com"",
    ""ts"": 1420303407
  },
  {
    ""event"": ""spam"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""opens"": [
        {
        ""ts"": 1365111111
        }
    ],
    ""clicks"": [
        {
        ""ts"": 1365111111,
        ""url"": ""http://mandrill.com""
        }
    ],
    ""state"": ""sent"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa""
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ts"": 1420303407
  },
  {
    ""event"": ""unsub"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""opens"": [
        {
        ""ts"": 1365111111
        }
    ],
    ""clicks"": [
        {
        ""ts"": 1365111111,
        ""url"": ""http://mandrill.com""
        }
    ],
    ""state"": ""sent"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa""
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ts"": 1420303407
  },
  {
    ""event"": ""reject"",
    ""msg"": {
    ""ts"": 1365109999,
    ""subject"": ""This an example webhook message"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""sender"": ""example.sender@mandrillapp.com"",
    ""tags"": [
        ""webhook-example""
    ],
    ""opens"": [],
    ""clicks"": [],
    ""state"": ""rejected"",
    ""metadata"": {
        ""user_id"": 111
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""_version"": ""exampleaaaaaaaaaaaaaaa""
    },
    ""_id"": ""fa239ebedd0a4831a3eefa59e880d8b2"",
    ""ts"": 1420303407
  },
  {
    ""type"": ""blacklist"",
    ""action"": ""add"",
    ""reject"": {
    ""reason"": ""hard-bounce"",
    ""detail"": ""Example detail"",
    ""last_event_at"": ""2014-02-01 12:43:56"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""created_at"": ""2014-01-15 11:32:19"",
    ""expires_at"": ""2020-04-02 12:09:18"",
    ""expired"": false,
    ""subaccount"": ""example_subaccount"",
    ""sender"": ""example.sender@mandrillapp.com""
    },
    ""ts"": 1420303407
  },
  {
    ""type"": ""blacklist"",
    ""action"": ""change"",
    ""reject"": {
    ""reason"": ""hard-bounce"",
    ""detail"": ""Example detail"",
    ""last_event_at"": ""2014-02-01 12:43:56"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""created_at"": ""2014-01-15 11:32:19"",
    ""expires_at"": ""2020-04-02 12:09:18"",
    ""expired"": false,
    ""subaccount"": ""example_subaccount"",
    ""sender"": ""example.sender@mandrillapp.com""
    },
    ""ts"": 1420303407
  },
  {
    ""type"": ""blacklist"",
    ""action"": ""remove"",
    ""reject"": {
    ""reason"": ""hard-bounce"",
    ""detail"": ""Example detail"",
    ""last_event_at"": ""2014-02-01 12:43:56"",
    ""email"": ""example.webhook@mandrillapp.com"",
    ""created_at"": ""2014-01-15 11:32:19"",
    ""expires_at"": ""2020-04-02 12:09:18"",
    ""expired"": false,
    ""subaccount"": ""example_subaccount"",
    ""sender"": ""example.sender@mandrillapp.com""
    },
    ""ts"": 1420303407
  },
  {
    ""type"": ""whitelist"",
    ""action"": ""add"",
    ""entry"": {
    ""email"": ""example.webhook@mandrillapp.com"",
    ""detail"": ""example details"",
    ""created_at"": ""2014-01-15 12:03:19""
    },
    ""ts"": 1420303407
  },
  {
    ""type"": ""whitelist"",
    ""action"": ""remove"",
    ""entry"": {
    ""email"": ""example.webhook@mandrillapp.com"",
    ""detail"": ""example details"",
    ""created_at"": ""2014-01-15 12:03:19""
    },
    ""ts"": 1420303407
  }
]";

        #endregion

        #region sample_webhook

        public const string sample_webhook =
            @"[{""event"":""send"",""msg"":{""ts"":1365109999,""subject"":""This an example webhook message"",""email"":""example.webhook@mandrillapp.com"",""sender"":""example.sender@mandrillapp.com"",""tags"":[""webhook-example""],""opens"":[],""clicks"":[],""state"":""sent"",""metadata"":{""user_id"":111},""_id"":""exampleaaaaaaaaaaaaaaaaaaaaaaaaa"",""_version"":""exampleaaaaaaaaaaaaaaa""},""_id"":""exampleaaaaaaaaaaaaaaaaaaaaaaaaa"",""ts"":1432328547},{""event"":""hard_bounce"",""msg"":{""ts"":1365109999,""subject"":""This an example webhook message"",""email"":""example.webhook@mandrillapp.com"",""sender"":""example.sender@mandrillapp.com"",""tags"":[""webhook-example""],""state"":""bounced"",""metadata"":{""user_id"":111},""_id"":""exampleaaaaaaaaaaaaaaaaaaaaaaaaa1"",""_version"":""exampleaaaaaaaaaaaaaaa"",""bounce_description"":""bad_mailbox"",""bgtools_code"":10,""diag"":""smtp;550 5.1.1 The email account that you tried to reach does not exist. Please try double-checking the recipient's email address for typos or unnecessary spaces.""},""_id"":""exampleaaaaaaaaaaaaaaaaaaaaaaaaa1"",""ts"":1432328547},{""event"":""open"",""msg"":{""ts"":1365109999,""subject"":""This an example webhook message"",""email"":""example.webhook@mandrillapp.com"",""sender"":""example.sender@mandrillapp.com"",""tags"":[""webhook-example""],""opens"":[{""ts"":1365111111}],""clicks"":[{""ts"":1365111111,""url"":""http:\/\/mandrill.com""}],""state"":""sent"",""metadata"":{""user_id"":111},""_id"":""exampleaaaaaaaaaaaaaaaaaaaaaaaaa2"",""_version"":""exampleaaaaaaaaaaaaaaa""},""_id"":""exampleaaaaaaaaaaaaaaaaaaaaaaaaa2"",""ip"":""127.0.0.1"",""location"":{""country_short"":""US"",""country"":""United States"",""region"":""Oklahoma"",""city"":""Oklahoma City"",""latitude"":35.4675598145,""longitude"":-97.5164337158,""postal_code"":""73101"",""timezone"":""-05:00""},""user_agent"":""Mozilla\/5.0 (Macintosh; U; Intel Mac OS X 10.6; en-US; rv:1.9.1.8) Gecko\/20100317 Postbox\/1.1.3"",""user_agent_parsed"":{""type"":""Email Client"",""ua_family"":""Postbox"",""ua_name"":""Postbox 1.1.3"",""ua_version"":""1.1.3"",""ua_url"":""http:\/\/www.postbox-inc.com\/"",""ua_company"":""Postbox, Inc."",""ua_company_url"":""http:\/\/www.postbox-inc.com\/"",""ua_icon"":""http:\/\/cdn.mandrill.com\/img\/email-client-icons\/postbox.png"",""os_family"":""OS X"",""os_name"":""OS X 10.6 Snow Leopard"",""os_url"":""http:\/\/www.apple.com\/osx\/"",""os_company"":""Apple Computer, Inc."",""os_company_url"":""http:\/\/www.apple.com\/"",""os_icon"":""http:\/\/cdn.mandrill.com\/img\/email-client-icons\/macosx.png"",""mobile"":false},""ts"":1432328547}]";

        #endregion

        #region sample sync event
        public const string sample_sync_event = @"[{""type"":""blacklist"",""action"":""add"",""reject"":{""reason"":""custom"",""detail"":null,""last_event_at"":""2017-01-01 16:56:20"",""email"":""blacklist@mandrilldotnet.org"",""created_at"":""2017-01-01 16:56:20"",""expires_at"":null,""expired"":false,""subaccount"":null,""sender"":null},""ts"":1483289780}]";

        #endregion

        public static readonly byte[] PngImage = Convert.FromBase64String(@"iVBORw0KGgoAAAANSUhEUgAAATYAAAE2CAYAAADrvL6pAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyRpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoTWFjaW50b3NoKSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDpCNjlCNzJDODA0MjUxMUUyOTAxOEU2QzVCRjBBOUJFNSIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDpCNjlCNzJDOTA0MjUxMUUyOTAxOEU2QzVCRjBBOUJFNSI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOkI2OUI3MkM2MDQyNTExRTI5MDE4RTZDNUJGMEE5QkU1IiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOkI2OUI3MkM3MDQyNTExRTI5MDE4RTZDNUJGMEE5QkU1Ii8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+PjVvLgAAS+dJREFUeNrsnQec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");
    }
}
