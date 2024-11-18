package com.dgames.nfchandlerlib;

import android.app.Activity;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.nfc.NfcAdapter;
import android.nfc.Tag;
import android.os.Build;

import java.util.LinkedList;
import java.util.Queue;

public class NfcHandler
{
    private Context context = null;
    private NfcAdapter mNfcAdapter = null;

    public NfcHandler() { }

    public void setContext(Context context)
    {
        this.context = context;
        this.mNfcAdapter = NfcAdapter.getDefaultAdapter(context);
    }

    public void onResume()
    {
        Intent intent = new Intent(context, context.getClass());
        intent.addFlags(Intent.FLAG_RECEIVER_REPLACE_PENDING);

        PendingIntent pendingIntent = PendingIntent.getActivity(context, 0, intent, PendingIntent.FLAG_MUTABLE);

        IntentFilter[] intentFilters = new IntentFilter[]{new IntentFilter(NfcAdapter.ACTION_TAG_DISCOVERED)};

        if (mNfcAdapter != null)
            mNfcAdapter.enableForegroundDispatch((Activity) context, pendingIntent, intentFilters, null);
    }

    public void onPause()
    {
        if (mNfcAdapter != null)
            mNfcAdapter.disableForegroundDispatch((Activity) context);
    }
}
